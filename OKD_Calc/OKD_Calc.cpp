/////////////////////////////////////////////////////////////////////
//
//  OKD_Calc.cpp
//  ���ZDLL�N���X
// 
//	2009-10-08	�쐬	�������r



#include "stdafx.h"
#include <cfloat>

#include "OKD_Calc.h"

////////////////////////////////////////////////////////////////////////////
#pragma warning( disable : 4996 )
#define _CRT_SECURE_NO_DEPRECATE

#ifdef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#undef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1
//strcopy��sprintf()�Ȃǂ�(�o�b�t�@�I�[�o�����΍�)���[�j���O��}�~����
#endif
////////////////////////////////////////////////////////////////////////////

using namespace std ;

//#include	"CalcDef.h"
#include	"CalcEngine.h"
#include	"CalcRpn.h"

#include	"Tool.h"


static string		strRpn = "";	//RPN�̕�����i���j��ێ�����
static string		strResult = "";

CalcEngine	Calc;
//static CalcRpn		CalcRpn;
CalcRpn		CalcRpn;

char ErrorString[ 512 ];
int		calcErrZero =0;	//DBL_MAX;


//���Z���ʗp
struct	stCalcResult {
#define MAX_CALCVAL_NUM	2000
	int		nCalcResult;
	std::string CalcResultName[MAX_CALCVAL_NUM];
	double CalcResult[MAX_CALCVAL_NUM];
	double *ptrDefCalcResultVal[MAX_CALCVAL_NUM];
};
struct stCalcResult sCalcResult;

//�ϐ��l�p
struct	stVariableVal {
#define MAX_VAL_NUM	2000
	int		nValResult;
	std::string ValName[MAX_VAL_NUM];
	double VariableVal[MAX_VAL_NUM];
	double *ptrDefVariableVal[MAX_VAL_NUM];
};
struct stVariableVal sVariableVal;

//Select���Z�p
struct	stSelect {
#define MAX_SELECTVAL_NUM	200
#define MAX_SELECT_IDX		100
	int		nSelectResult;
	std::string CalcName[MAX_SELECTVAL_NUM];
	std::string SelectResultName[MAX_SELECTVAL_NUM];
	double SelectResult[MAX_SELECTVAL_NUM];
	std::string SelectFlagName[MAX_SELECTVAL_NUM];
	int SelectOffset[MAX_SELECTVAL_NUM];
	int SelectItemCount[MAX_SELECTVAL_NUM];
//ADD 2009-11-17
	int SetExpressionFlag[MAX_SELECTVAL_NUM];
//ADD 2009-11-17
};
struct stSelect sSelect;


namespace OKD {
namespace Common {


// �R���X�g���N�^
CalcByValue::CalcByValue()
{
	//strRpn = "";
	//strResult = "";
	for( ; strRpn.size() > 0 ; ){
		strRpn.erase();
	}
	for( ; strResult.size() > 0 ; ){
		strResult.erase();
	}

	//�ر
	InitBuf();
	Calc.init(); 

	return;
}


//----------------------------------------------------------------------------
// FUNCTION NAME : Initialize
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�萔�E�ϐ��E���Z���ݒ���̏�����
//----------------------------------------------------------------------------
void CalcByValue::Initialize()	// �߂�l�Ȃ�
{
	//strRpn = "";
	//strResult = "";
	for( ; strRpn.size() > 0 ; ){
		strRpn.erase();
	}
	for( ; strResult.size() > 0 ; ){
		strResult.erase();
	}

	//�ر
	InitBuf();
	Calc.init(); 

	return;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : InitBuf
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�Ǘ��ޯ̧�̸ر
//----------------------------------------------------------------------------
void CalcByValue::InitBuf(void)
{
	int i;
	//Select���Z�p
	for(i=0; i<MAX_SELECTVAL_NUM; i++){
		sSelect.CalcName[i] = "";
		sSelect.SelectResultName[i] = "";
		sSelect.SelectResult[i] = 0.0;
		sSelect.SelectFlagName[i] = "";
		sSelect.SelectOffset[i] = 0;
		sSelect.SelectItemCount[i] = 0;
//ADD 2009-11-17
		sSelect.SetExpressionFlag[i] = 0;
//ADD 2009-11-17
	}
	sSelect.nSelectResult = 0;

}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetVariable
//
// INPUT OUTPUT :
//		int			numVariable			(I)	�ϐ��̑���
//		string		strVariableName[]	(I)	�ϐ����̔z��
//		double *	ptrVariableVal[]	(I)	�ϐ��l�̊i�[�̈�̃|�C���^�̔z��
//		string &	strErrorMessage		(O)	�G���[��񕶎���i�G���[���e�E���̂���萔���Ȃǁj
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϐ����ƕϐ��l�i�[�̈�̐ݒ�
//----------------------------------------------------------------------------
int CalcByValue::SetVariable(
		int numVariable,
		std::string strVariableName[],
		double valVariableVal[],
		std::string &strErrorMessage)
{
	int iRet = rErrOK;
	int i, j;
	int k;
	int iFind;
	int iSelectResult = sSelect.nSelectResult;
	std::string name;
	std::string name2;
	strErrorMessage = "NoError";

	try {
		for(i=0; i<numVariable; i++){
			if( strVariableName[i].empty() != TRUE ){

				//�啶���ϊ�
				Tool_ChgUpper( strVariableName[i], name);

				Calc.SetVariable(name, valVariableVal[i]); 
#ifdef DEBUG_PRINT1
			printf("name=%s. %f\n", name.c_str(), valVariableVal[i] );
#endif

				if( iSelectResult == 0 ){
					//Select�̊m�F		"PHASE_DIST_SEL.1";
					if( ( k = name.find( "." ) ) >= 0 ) {
						name2 = name.substr(0, k);	
						//"PHASE_DIST_SEL
						iFind = -1;
						for(j=0; j<sSelect.nSelectResult; j++){
							if( sSelect.SelectResultName[j] == name2 ){
								iFind = j;
								break;
							}
						}
						if( iFind == -1 ){
							//��\���݂̂�o�^
							sSelect.SelectResultName[sSelect.nSelectResult] = name2;		//PHASE_DIST_SEL
							sSelect.SelectItemCount[sSelect.nSelectResult]++;				//��@PHASE_DIST_SEL.1
							sSelect.nSelectResult++;

						}else{
							sSelect.SelectItemCount[iFind]++;								//��@PHASE_DIST_SEL.2 ,PHASE_DIST_SEL.3
						}
					}
				}
			}else{
				strErrorMessage = "Empty VariableName";
				iRet = rErrNG;
				break;
			}
		}
	}
	catch(...) {
		strErrorMessage = "Empty VariableName";
		iRet = rErrNG;
	}

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetExpression
//
// INPUT OUTPUT :
//		int			numExpression		(I)	���Z���̑���
//		string		strCalcName[]		(I)	���Z�����̔z��
//		string		strExpression[]		(I)	���Z���̔z��
//		string &	strErrorMessage		(O)	�G���[��񕶎���i�G���[���e�E���̂���萔���Ȃǁj
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		���Z�����Ɖ��Z���Ɖ��Z���ʒl�i�[�̈�̐ݒ�
//----------------------------------------------------------------------------
int CalcByValue::SetExpression(
		int numExpression,
		std::string strCalcName[],
		std::string strExpression[],
		std::string &strErrorMessage)
{
	int iRet = rErrOK;
	int i;
	int k, j;
	int iFind;
	char	tmp[512];
	char	temp[1024] ;
	int		iErr;
	std::string name;
	std::string name2;
	std::string rpnname;
	std::string rpnname2;
	std::string strflag;
	strErrorMessage = "NoError";

    //��
	for( ; strRpn.size() > 0 ; ){
		strRpn.erase() ;
	}

	try {
		for(i=0; i<numExpression; i++){
			if( strCalcName[i].empty() != TRUE && strExpression[i].empty() != TRUE){

				//�啶���ϊ�
				Tool_ChgUpper( strCalcName[i], name);
				Tool_ChgUpper( strExpression[i], rpnname);


//ADD 2009-11-17
				//Select�̊m�F		"PHASE_DIST_SEL2.1";
				if( ( k = name.find( "." ) ) >= 0 ) {
					name2 = name.substr(0, k);	
					//"PHASE_DIST_SEL2
					iFind = -1;
					for(j=0; j<sSelect.nSelectResult; j++){
						if( sSelect.SelectResultName[j] == name2 ){
							iFind = j;
							break;
						}
					}
					if( iFind == -1 ){
						//��\���݂̂�o�^
						sSelect.SelectResultName[sSelect.nSelectResult] = name2;		//PHASE_DIST_SEL2
						sSelect.SelectItemCount[sSelect.nSelectResult]++;				//��@PHASE_DIST_SEL2.1
						sSelect.SetExpressionFlag[sSelect.nSelectResult] = 1;
						sSelect.nSelectResult++;

					}else{
						sSelect.SelectItemCount[iFind]++;								//��@PHASE_DIST_SEL2.2 ,PHASE_DIST_SEL2.3
						sSelect.SetExpressionFlag[iFind] = 1;
					}
				}
//ADD 2009-11-17

				//Select�̊m�F
				if( ( k = rpnname.find( "SELECT" ) ) >= 0 ) {
					//��������

					//��@"SELECT(PHASE_DIST_SEL,DIST_TYPE,1)"  ----> "PHASE_DIST_SEL,DIST_TYPE,1"
					//            |                        | 
					//          k+6+1             rpnname.size()-(k+6+2)
					rpnname2 = rpnname.substr(k+6+1, rpnname.size()-(k+6+2));

					unsigned int k = stringToken(rpnname2, ",");
					std::vector< string > ss( k ) ;
					for(unsigned int ii = 0; ii< k; ii++){
						ss.at(ii) = strTokenBuf[ii];
					}
					iFind = -1;
					if( k >= 3 ){
						for(j=0; j< sSelect.nSelectResult; j++){
							strflag = ss.at(0);
							if( sSelect.SelectResultName[j] == strflag ){
								iFind = j;
								strflag = ss.at(1);
								sSelect.SelectFlagName[j] = strflag;

								strflag = ss.at(2);
								sSelect.SelectOffset[j] = atoi(strflag.c_str());

								sSelect.CalcName[j] = name;
								break;
							}
						}
					}
					if( iFind == -1 ){
						//error
						strErrorMessage = "SELECT(....) Keyword is not found";
						iRet = rErrNG;
						return iRet;
					}

				}else{

					//���ق�o�^
					Calc.SetVariable(name, 0.0F); 
					//���̐���
					sprintf(tmp, "%s=%s", name.c_str(), strExpression[i].c_str()); 
					//���Z������Ǔo�^
					iErr = CalcRpn.strRPN( tmp, temp, sizeof(temp));
					if( iErr ){
						strErrorMessage = temp;
						iRet = rErrNG;
#ifdef DEBUG_PRINT1
						printf("\n\nError=%s\n", temp);
#endif
						break;
					}else{
						//���قƎ���o�^
						Calc.SetRpn(name, strExpression[i]);
					}
#ifdef DEBUG_PRINT1
					printf("name=%s. %s\n  Calc�ϊ���=%s\n", strCalcName[i].c_str(), strExpression[i].c_str(), temp);
#endif
					//�ϊ��ς̎���o�^
					strRpn = strRpn + temp +"\r\n" ;

				}

			}else{
				strErrorMessage = "Empty CalcName or Expression";
				iRet = rErrNG;
				break;
			}
		}
	}
	catch(...) {
		strErrorMessage = "Empty CalcName or Expression";
		iRet = rErrNG;
	}

#ifdef DEBUG_PRINT3
	if( iRet == rErrOK ){
		printf(">>>>>> CalcByValue::SetExpression()  strRpn.size()=%d\n", strRpn.size());
		printf(">>>>>> CalcByValue::SetExpression()  strRpn=%s\n", strRpn.c_str());
	}
#endif

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : Execute
//
// INPUT OUTPUT :
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		���Z���s
//----------------------------------------------------------------------------
int CalcByValue::Execute(
		std::string &strErrorMessage)
{
	unsigned int i;
	char	temp[MAXSTRRPN] ;
	char	*s ;
	string	token ;
	int iRet = rErrOK;
	strErrorMessage = "NoError";

	if( strRpn.size() <= 0 ){
		strErrorMessage = "Calculation Table is not set.";
		iRet = rErrNG;
		return iRet;
	}

	for( ; strResult.size() > 0 ; ){
		strResult.erase();
	}

Timer_Start();

	///////////////////////////////////////////////
	//Select���ް����
	SetSelectDat();

double tTime = 0;
Timer_Stop(&tTime);
#ifdef DEBUG_PRINT5
	printf("tTime=%f\n", tTime);
#endif

	///////////////////////////////////////////////
	//
Timer_Start();
	unsigned int k = stringToken(strRpn, "\r\n");
	std::vector< string > ss( k ) ;
	for(unsigned int ii = 0; ii< k; ii++){
		ss.at(ii) = strTokenBuf[ii];
	}
Timer_Stop(&tTime);
#ifdef DEBUG_PRINT5
	printf("�؂�o���Ԃ�=%f\n", tTime);
#endif

	for( i = 0; i< k; i++){
		token = ss.at(i);
		temp[0] = '\0';

		if( token[0] != RPN_REMARK ){
    		Calc.rpnOperation( token.c_str(), temp, sizeof(temp)-2 );
	        s = strchr( temp, '=' );
			if( s != NULL ){
    		    *s = ',';
			}
			strcat( temp , "," );
			strResult.append( temp );
		}
	}

Timer_Start();
//ADD 2009-11-17
	///////////////////////////////////////////////
	//Select���ް����
	SetSelectDat2();
	SetSelectDat();
//ADD 2009-11-17
Timer_Stop(&tTime);
#ifdef DEBUG_PRINT5
	printf("tTime2=%f\n", tTime);
#endif

#ifdef DEBUG_PRINT3
	if( strResult.size() > 0 ){
		printf("\n\CalcByValue::Execute()  strResult=%s\n", strResult.c_str());
	}
#endif
#ifdef DEBUG_PRINT4
	if( strResult.size() > 0 ){
		printf(".");
	}
#endif

	return iRet;
}
	
//----------------------------------------------------------------------------
// FUNCTION NAME : GetCalcResult
//
// INPUT OUTPUT :
//		string		strCalcResultName	(I)	���Z����
//		string &	valCalResult		(O)	���Z���ʒl
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		���Z���ʎ擾
//----------------------------------------------------------------------------
int CalcByValue::GetCalcResult(
		std::string strCalcResultName,
		double &valCalResult)
{
	int iRet = rErrOK;

	if( strCalcResultName.empty() == TRUE){
		iRet = rErrNG;
		return iRet;
	}

	//�w�����ق̉��Z���ʂ��擾
	valCalResult = Calc.getSymbol(strCalcResultName);

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : GetExcelFormatExpression
//
// INPUT OUTPUT :
//		string		strCalcName			(I)	���Z����
//		string &	strExpression		(O)	EXCEL�`���ɕϊ��������Z����
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Excel�p���Z���擾
//----------------------------------------------------------------------------
int CalcByValue::GetExcelFormatExpression(
		std::string strCalcName,
		std::string &strExpression)
{
	int iRet = rErrOK;
	std::string strRpn;
	std::string strName;
	std::string strTmp;

	if( strCalcName.empty() == TRUE){
		strExpression = "Empty CalcName";		//"No Label";
		iRet = rErrNG;
		return iRet;
	}

	//�啶���ϊ�
	Tool_ChgUpper( strCalcName, strName);
	if( GetSelectDat(strName, strTmp) ){
		//no error
		strExpression = strName + "=" + strTmp;
		return iRet;
	}

	//���قƎ���o�^
	int iErr = Calc.GetRpn(strName, strRpn);
	if( iErr ){
		//no error
		strExpression = strName + "= " + strRpn;
	}else{
		strExpression = "No Label";
	}
	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetSelectDat
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Select���ް����
//----------------------------------------------------------------------------
void CalcByValue::SetSelectDat(void)
{
	int i;
	int Offset;
	std::string strFlag;
	std::string strName;
	std::string strCalcName;
	char tmp[256];
	int valResult;
	double valCalcResult;
	for(i=0; i< sSelect.nSelectResult; i++){
		Offset  = sSelect.SelectOffset[i]; 
		strFlag = sSelect.SelectFlagName[i]; 
		strName = sSelect.SelectResultName[i];
		strCalcName = sSelect.CalcName[i]; 
		
		valResult = (int)Calc.getSymbol(strFlag);
		sprintf(tmp, "%s.%d", strName.c_str(), (Offset + valResult) ); 

		valCalcResult = Calc.getSymbol(tmp);
		//Select�̌��ʂ�Ă���
		Calc.setSymbol(strCalcName, valCalcResult); 
	}
}

//ADD 2009-11-17
void CalcByValue::SetSelectDat2(void)
{
	int i;
	int k;
	int Offset;
	std::string strFlag;
	std::string strName;
	std::string strCalcName;
	char tmp[256];
	int valResult;
	double valCalcResult;

	for(i=0; i< sSelect.nSelectResult; i++){
		Offset  = sSelect.SelectOffset[i]; 
		strFlag = sSelect.SelectFlagName[i]; 
		strName = sSelect.SelectResultName[i];
		strCalcName = sSelect.CalcName[i]; 

		//�������͎��Ƃ��Ē�`
		if( sSelect.SetExpressionFlag[i] > 0 ){
			for(k=0; k< sSelect.SelectItemCount[i]; k++){
				sprintf(tmp, "%s.%d", strName.c_str(), k+1 ); 
				valCalcResult = Calc.getSymbol(tmp);
				//���Z���ʂ�Ă���
				Calc.setSymbol(tmp, valCalcResult); 
			}
		}else{
			//�������͕ϐ��Ƃ��Ē�`
			valResult = (int)Calc.getSymbol(strFlag);
			sprintf(tmp, "%s.%d", strName.c_str(), (Offset + valResult) ); 
	
			valCalcResult = Calc.getSymbol(tmp);
			//Select�̌��ʂ�Ă���
			Calc.setSymbol(strCalcName, valCalcResult); 
		}
	}
}
//ADD 2009-11-17

//----------------------------------------------------------------------------
// FUNCTION NAME : GetSelectDat
//
// INPUT OUTPUT :
//		string		name				(I)	���Z����
//		string &	ResultName			(O)	EXCEL�`���ɕϊ��������Z��
//		int			(RETURN)			(R)	�߂�l(1�FSelect�֘A�A0�F���̑��̒ʏ�̎�)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Select�֘A�̎w�薼���牉�Z����W�J����
//----------------------------------------------------------------------------
int CalcByValue::GetSelectDat(std::string name, std::string& ResultName)
{
	int i, k;
	int Offset;
	int SelectItemCount;
	std::string strFlag;
	std::string strName;
	std::string strCalcName;
	std::string tmp="";
	int val;
	char c[256];

	ResultName="";

	//"PHASE_DIST=IF(DIST_TYPE=0,PHASE_DIST_SEL.1,IF(DIST_TYPE=1,PHASE_DIST_SEL.2,0))"
	//"PHASE_DIST=IF(DIST_TYPE=0,PHASE_DIST_SEL.1,IF(DIST_TYPE=1,PHASE_DIST_SEL.2,IF(DIST_TYPE=2,PHASE_DIST_SEL.3,0)))"

	for(i=0; i< sSelect.nSelectResult; i++){
		strCalcName = sSelect.CalcName[i]; 
		Offset  = sSelect.SelectOffset[i]; 
		strFlag = sSelect.SelectFlagName[i]; 
		strName = sSelect.SelectResultName[i];
		SelectItemCount = sSelect.SelectItemCount[i];								//��@PHASE_DIST_SEL.1 ,PHASE_DIST_SEL.2 ,PHASE_DIST_SEL.3
		val = 0;
		if( strCalcName == name ){

			tmp = "";
			for(k=0; k< SelectItemCount; k++){
				if( k==0 ){
					sprintf(c, "IF(%s=%d,%s.%d", strFlag.c_str(), val, strName.c_str(), (k+1));
				}else{
					sprintf(c, ",IF(%s=%d,%s.%d", strFlag.c_str(), val, strName.c_str(), (k+1));
				}
				tmp = tmp + c ; 
				val++;
			}

			tmp = tmp + ",0";
			for(k=0; k< SelectItemCount; k++){
				tmp = tmp + ")";
			}

			ResultName = tmp;
			return 1;
		}
	}

	return 0;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////


// �R���X�g���N�^
CalcByMemory::CalcByMemory()
{
	//strRpn = "";
	//strResult = "";
	for( ; strRpn.size() > 0 ; ){
		strRpn.erase();
	}
	for( ; strResult.size() > 0 ; ){
		strResult.erase();
	}

	//�ر
	InitBuf();
	Calc.init(); 

	return;
}

// �f�X�g���N�^
CalcByMemory::~CalcByMemory()
{
    //_CrtDumpMemoryLeaks() ;
	return;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : InitBuf
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�Ǘ��ޯ̧�̸ر
//----------------------------------------------------------------------------
void CalcByMemory::InitBuf(void)
{
	int i;
	//���Z���ʗp
	for(i=0; i<MAX_CALCVAL_NUM; i++){
		sCalcResult.CalcResultName[i] = "";
		//���Z���ʒl�̊i�[�̈�̃|�C���^�̔z��
		sCalcResult.ptrDefCalcResultVal[i] = &sCalcResult.CalcResult[i];
	}
	sCalcResult.nCalcResult = 0;


	//�ϐ��l�p
	for(i=0; i<MAX_VAL_NUM; i++){
		sVariableVal.ValName[i] = "";
		//���Z���ʒl�̊i�[�̈�̃|�C���^�̔z��
		sVariableVal.ptrDefVariableVal[i] = &sVariableVal.VariableVal[i];
	}
	sVariableVal.nValResult = 0;

	//Select���Z�p
	for(i=0; i<MAX_SELECTVAL_NUM; i++){
		sSelect.CalcName[i] = "";
		sSelect.SelectResultName[i] = "";
		sSelect.SelectResult[i] = 0.0;
		sSelect.SelectFlagName[i] = "";
		sSelect.SelectOffset[i] = 0;
		sSelect.SelectItemCount[i] = 0;
//ADD 2009-11-17
		sSelect.SetExpressionFlag[i] = 0;
//ADD 2009-11-17
	}
	sSelect.nSelectResult = 0;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : Initialize
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�萔�E�ϐ��E���Z���ݒ���̏�����
//----------------------------------------------------------------------------
void CalcByMemory::Initialize()			// �߂�l�Ȃ�
{

	//strRpn = "";
	//strResult = "";
	for( ; strRpn.size() > 0 ; ){
		strRpn.erase();
	}
	for( ; strResult.size() > 0 ; ){
		strResult.erase();
	}

	//�ر
	InitBuf();
	Calc.init(); 

	return;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetConstant
//
// INPUT OUTPUT :
//		int			numConstant			(I)	�萔�̑���
//		string		strConstantName[]	(I)	�萔���̔z��
//		double		valConstant[]		(I)	�萔�l�̔z��
//		string &	strErrorMessage		(O)	�G���[��񕶎���i�G���[���e�E���̂���萔���Ȃǁj
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�萔���ƒ萔�l�̐ݒ�
//----------------------------------------------------------------------------
int CalcByMemory::SetConstant(
		int numConstant,
		std::string strConstantName[],
		double valConstant[],
		std::string &strErrorMessage)
{
	int iRet = rErrOK;
	int i;
	strErrorMessage = "NoError";

#ifdef DEBUG_PRINT2
	printf("CalcByMemory::SetConstant()\n");
#endif

	try {
		for(i=0; i<numConstant; i++){
			if( strConstantName[i].empty() != TRUE ){
				Calc.SetVariable(strConstantName[i], valConstant[i]); 
#ifdef DEBUG_PRINT1
			printf("name=%s. %10.10f\n", strConstantName[i].c_str(), valConstant[i] );
#endif
			}else{
				strErrorMessage = "Empty ConstantName";
				iRet = rErrNG;
				break;
			}
		}
	}
	catch(...) {
		strErrorMessage = "Empty ConstantName";
		iRet = rErrNG;
	}

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetVariable
//
// INPUT OUTPUT :
//		int			numVariable			(I)	�ϐ��̑���
//		string		strVariableName[]	(I)	�ϐ����̔z��
//		double *	ptrVariableVal[]	(I)	�ϐ��l�̊i�[�̈�̃|�C���^�̔z��
//		string &	strErrorMessage		(O)	�G���[��񕶎���i�G���[���e�E���̂���萔���Ȃǁj
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϐ����ƕϐ��l�i�[�̈�̐ݒ�
//----------------------------------------------------------------------------
int CalcByMemory::SetVariable(
		int numVariable,
		std::string strVariableName[],
		double *ptrVariableVal[],
		std::string &strErrorMessage)
{
	int iRet = rErrOK;
	int i;
	int k,j;
	int iFind;
	std::string name;
	std::string name2;
	strErrorMessage = "NoError";

#ifdef DEBUG_PRINT2
	printf("CalcByMemory::SetVariable()\n");
#endif

	//
	for(i=0; i<MAX_VAL_NUM; i++){
		sVariableVal.ValName[i] = "";
	}
	sVariableVal.nValResult = 0;

	try {
		for(i=0; i<numVariable; i++){
			if( strVariableName[i].empty() != TRUE ){

				//�啶���ϊ�
				Tool_ChgUpper( strVariableName[i], name);

				if( ptrVariableVal[i] != NULL ){
					Calc.SetVariable(name, *(ptrVariableVal[i])); 
#ifdef DEBUG_PRINT1
					printf("name=%s. %f\n", name.c_str(), *(ptrVariableVal[i]) );
#endif
					//���Z���ʒl�̊i�[�̈�̃|�C���^�̔z��
					sVariableVal.ptrDefVariableVal[i] = ptrVariableVal[i];

//debug				*(sVariableVal.ptrDefVariableVal[i])=i+1+100;

				}else{
					//�ް��ޯ̧��NULL�Ȃ̂�
					Calc.SetVariable(name, 0.0); 
					sVariableVal.ptrDefVariableVal[i] = NULL;
				}

				sVariableVal.ValName[i] = name;
				sVariableVal.nValResult++;

				//Select�̊m�F		"PHASE_DIST_SEL.1";
				if( ( k = name.find( "." ) ) >= 0 ) {
					name2 = name.substr(0, k);	
					//"PHASE_DIST_SEL
					iFind = -1;
					for(j=0; j<sSelect.nSelectResult; j++){
						if( sSelect.SelectResultName[j] == name2 ){
							iFind = j;
							break;
						}
					}
					if( iFind == -1 ){
						//��\���݂̂�o�^
						sSelect.SelectResultName[sSelect.nSelectResult] = name2;		//PHASE_DIST_SEL
						sSelect.SelectItemCount[sSelect.nSelectResult]++;				//��@PHASE_DIST_SEL.1
						sSelect.nSelectResult++;

					}else{
						sSelect.SelectItemCount[iFind]++;								//��@PHASE_DIST_SEL.2 ,PHASE_DIST_SEL.3
					}
				}

			}else{
				strErrorMessage = "Empty VariableName";
				iRet = rErrNG;
				break;
			}
		}
	}
	catch(...) {
		strErrorMessage = "Empty VariableName";
		iRet = rErrNG;
	}

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetExpression
//
// INPUT OUTPUT :
//		int			numExpression		(I)	���Z���̑���
//		string		strCalcName[]		(I)	���Z�����̔z��
//		string		strExpression[]		(I)	���Z���̔z��
//		double *	ptrCalcResult[]		(I)	���Z���ʒl�̊i�[�̈�̃|�C���^�̔z��i�ȗ��\�j
//		string &	strErrorMessage		(O)	�G���[��񕶎���i�G���[���e�E���̂���萔���Ȃǁj
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		���Z�����Ɖ��Z���Ɖ��Z���ʒl�i�[�̈�̐ݒ�
//----------------------------------------------------------------------------
int CalcByMemory::SetExpression(
		int numExpression,
		std::string strCalcName[],
		std::string strExpression[],
		double *ptrCalcResult[],
		std::string &strErrorMessage)
{
	int iRet = rErrOK;
	int i;
	int k,j;
	int iFind;
	char	tmp[512];
	char	temp[1024] ;
	int		iErr;
	std::string name;
	std::string name2;
	std::string rpnname;
	std::string rpnname2;
	std::string strflag;

	strErrorMessage = "NoError";

#ifdef DEBUG_PRINT2
	printf("CalcByMemory::SetExpression()\n");
#endif

    //��
	for( ; strRpn.size() > 0 ; ){
		strRpn.erase() ;
	}
	for(i=0; i<MAX_CALCVAL_NUM; i++){
		sCalcResult.CalcResultName[i] = "";
	}
	sCalcResult.nCalcResult = 0;

	if( numExpression > MAX_CALCVAL_NUM ){
		strErrorMessage = "Subscript out of range";
		iRet = rErrNG;
		return iRet;
	}

	//
	try {
		for(i=0; i<numExpression; i++){
			if( strCalcName[i].empty() != TRUE && strExpression[i].empty() != TRUE){
				//�啶���ϊ�
				Tool_ChgUpper( strCalcName[i], name);
				Tool_ChgUpper( strExpression[i], rpnname);

//ADD 2009-11-17
				//Select�̊m�F		"PHASE_DIST_SEL2.1";
				if( ( k = name.find( "." ) ) >= 0 ) {
					name2 = name.substr(0, k);	
					//"PHASE_DIST_SEL2
					iFind = -1;
					for(j=0; j<sSelect.nSelectResult; j++){
						if( sSelect.SelectResultName[j] == name2 ){
							iFind = j;
							break;
						}
					}
					if( iFind == -1 ){
						//��\���݂̂�o�^
						sSelect.SelectResultName[sSelect.nSelectResult] = name2;		//PHASE_DIST_SEL2
						sSelect.SelectItemCount[sSelect.nSelectResult]++;				//��@PHASE_DIST_SEL2.1
						sSelect.SetExpressionFlag[sSelect.nSelectResult] = 1;
						sSelect.nSelectResult++;

					}else{
						sSelect.SelectItemCount[iFind]++;								//��@PHASE_DIST_SEL2.2 ,PHASE_DIST_SEL2.3
						sSelect.SetExpressionFlag[iFind] = 1;
					}
				}
//ADD 2009-11-17

				//Select�̊m�F
				if( ( k = rpnname.find( "SELECT" ) ) >= 0 ) {
					//��������

					//��@"SELECT(PHASE_DIST_SEL,DIST_TYPE,1)"  ----> "PHASE_DIST_SEL,DIST_TYPE,1"
					//            |                        | 
					//          k+6+1             rpnname.size()-(k+6+2)
					rpnname2 = rpnname.substr(k+6+1, rpnname.size()-(k+6+2));

					j = stringToken(rpnname2, ",");
					std::vector< string > ss( j ) ;
					for(int ii = 0; ii< j; ii++){
						ss.at(ii) = strTokenBuf[ii];
					}
					if( j >= 3 ){
						for(j=0; j< sSelect.nSelectResult; j++){
							strflag = ss.at(0);
							if( sSelect.SelectResultName[j] == strflag ){
								iFind = j;
								strflag = ss.at(1);
								sSelect.SelectFlagName[j] = strflag;

								strflag = ss.at(2);
								sSelect.SelectOffset[j] = atoi(strflag.c_str());

								sSelect.CalcName[j] = name;
								break;
							}
						}
					}
					if( iFind == -1 ){
						//error
						strErrorMessage = "SELECT(....) Keyword is not found";
						iRet = rErrNG;
						return iRet;
					}

				}else{
					//���ق�o�^
					Calc.SetVariable(name, 0.0F); 
					//���̐���
					sprintf(tmp, "%s=%s", name.c_str(), strExpression[i].c_str()); 
					//���Z������Ǔo�^
					iErr = CalcRpn.strRPN( tmp, temp, sizeof(temp));
					if( iErr ){
						strErrorMessage = temp;
						iRet = rErrNG;
#ifdef DEBUG_PRINT1
						printf("\n\nError=%s\n", temp);
#endif
						break;
					}else{
						//���قƎ���o�^
						Calc.SetRpn(name, strExpression[i]);
					}
#ifdef DEBUG_PRINT1
					printf("name=%s. %s\n  Calc�ϊ���=%s\n", name.c_str(), strExpression[i].c_str(), temp);
#endif
					//�ϊ��ς̎���o�^
					strRpn = strRpn + temp +"\r\n" ;
				}
				
				//���Z���ʒl�̊i�[�̈�̃|�C���^�̔z��
				if( ptrCalcResult[i] != NULL ){
					sCalcResult.ptrDefCalcResultVal[i] = ptrCalcResult[i];
				}else{
					sCalcResult.ptrDefCalcResultVal[i] = NULL;
				}
				sCalcResult.CalcResultName[i] = name;
				sCalcResult.nCalcResult++;

			}else{
				strErrorMessage = "Empty CalcName or Expression";
				iRet = rErrNG;
				break;
			}
		}
	}
	catch(...) {
		strErrorMessage = "Empty CalcName or Expression";
		iRet = rErrNG;
	}

	//debug
	//*ptrDefCalcResultVal[0] = 12345.6789;
#ifdef DEBUG_PRINT3
	if( iRet == rErrOK ){
		printf("\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n", strRpn.size());
		printf("CalcByValue::SetExpression()  strRpn.size()=%d\n", strRpn.size());
		printf("CalcByValue::SetExpression()  strRpn=%s\n", strRpn.c_str());
		printf(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n", strRpn.size());
	}
#endif

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : Execute
//
// INPUT OUTPUT :
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		���Z���s
//----------------------------------------------------------------------------
int CalcByMemory::Execute(
		std::string &strErrorMessage)
{
	unsigned int i;
	unsigned int k;
	char	temp[MAXSTRRPN] ;
	char	*s ;
	string	token ;
	string  name;
	int iRet = rErrOK;
	strErrorMessage = "NoError";

double tTime = 0;
#ifdef DEBUG_PRINT2
	printf("CalcByMemory::Execute()\n");
#endif

//Timer_Start();

	if( strRpn.size() <= 0 ){
		strErrorMessage = "Calculation Table is not set.";
		iRet = rErrNG;
		return iRet;
	}

	for( ; strResult.size() > 0 ; ){
		strResult.erase();
	}

//Timer_Start();

	///////////////////////////////////////////////
	//��ʂ��ް��p���ޯ̧����ŐV�������Ă�����
	SetNewVariableDat();

//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("0Time=%f\n", tTime);
//#endif

	///////////////////////////////////////////////
	//Select���ް����
	SetSelectDat();

//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	//printf("��=%s\n", strRpn.c_str());
//	printf("tTime=%f\n", tTime);
//#endif

	///////////////////////////////////////////////
	//
Timer_Start();
	i = stringToken(strRpn, "\r\n");
	std::vector< string > ss( i ) ;
	for(unsigned int ii = 0; ii< i; ii++){
		ss.at(ii) = strTokenBuf[ii];
	}
Timer_Stop(&tTime);
#ifdef DEBUG_PRINT5
	printf("�؂�o���܂�=%f\n", tTime);
#endif

double tTimeAdd = 0;
//Timer_Start();
	for( k = 0; k< i; k++){
		token = ss.at(k);
		temp[0] = '\0';
//Timer_Start();

		if( token[0] != RPN_REMARK ){
    		Calc.rpnOperation( token.c_str(), temp, sizeof(temp)-2 );
	        s = strchr( temp, '=' );
			if( s != NULL ){
    		    *s = ',';
			}
			strcat( temp , "," );
			strResult.append( temp );
			//printf("Calc.rpnOperation()��=%s\n", temp);
//Timer_Stop(&tTime);
//tTimeAdd += tTime;
//#ifdef DEBUG_PRINT5
//	printf("Calc.rpnOperation()��=%f\n", tTime);
//#endif
		}
	}
//#ifdef DEBUG_PRINT5
//	printf("Calc.rpnOperation()�@����=%f\n", tTimeAdd);
//#endif

//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("Calc.rpnOperation()��=%f\n", tTime);
//#endif

//ADD 2009-11-17
	///////////////////////////////////////////////
	//Select���ް����
//Timer_Start();
	SetSelectDat2();
	SetSelectDat();
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("tTime2=%f\n", tTime);
//#endif
//ADD 2009-11-17

//Timer_Start();
	///////////////////////////////////////////////
	//���Z���ʒl�̊i�[�̈���߲���z��ɾ�Ă���
	SetCalcResultDat();
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("�߲���z��ɾ�Ă�=%f\n", tTime);
//#endif

#ifdef DEBUG_PRINT3
	if( strResult.size() > 0 ){
		printf("\n\nCalcByValue::Execute()  strResult=%s\n", strResult.c_str());
	}
#endif
#ifdef DEBUG_PRINT4
	if( strResult.size() > 0 ){
		printf(".");
	}
#endif

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : GetCalcResult
//
// INPUT OUTPUT :
//		string		strCalcResultName	(I)	���Z����
//		string &	valCalResult		(O)	���Z���ʒl
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		���Z���ʎ擾
//----------------------------------------------------------------------------
int CalcByMemory::GetCalcResult(		// �߂�l(0�F�����A1�F���s)
		std::string strCalcResultName,		// ���Z����
		double &valCalResult)				// ���Z���ʒl
{
	int iRet = rErrOK;

	if( strCalcResultName.empty() == TRUE){
		iRet = rErrNG;
		return iRet;
	}

	//�w�����ق̉��Z���ʂ��擾
	valCalResult = Calc.getSymbol(strCalcResultName);

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : GetExcelFormatExpression
//
// INPUT OUTPUT :
//		string		strCalcName			(I)	���Z����
//		string &	strExpression		(O)	EXCEL�`���ɕϊ��������Z����
//		int			(RETURN)			(R)	�߂�l(0�F�����A1�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Excel�p���Z���擾
//----------------------------------------------------------------------------
int CalcByMemory::GetExcelFormatExpression(
		std::string strCalcName,
		std::string &strExpression)
{
	int iRet = rErrOK;
	std::string strRpn;
	std::string strName;
	std::string strTmp;

	if( strCalcName.empty() == TRUE){
		strExpression = "Empty CalcName";		//"No Label";
		iRet = rErrNG;
		return iRet;
	}

	//�啶���ϊ�
	Tool_ChgUpper( strCalcName, strName);
	if( GetSelectDat(strName, strTmp) ){
		//no error
		strExpression = strName + "=" + strTmp;
		return iRet;
	}

	//���قƎ���o�^
	int iErr = Calc.GetRpn(strName, strRpn);
	if( iErr ){
		//no error
		strExpression = strName + "=" + strRpn;
	}else{
		strExpression = "No Label";
	}
	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetNewVariableDat
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		��ʂ��ް��p���ޯ̧����ŐV�������Ă�����
//----------------------------------------------------------------------------
void CalcByMemory::SetNewVariableDat(void)
{
	int i;
	string  name;
	double valVariableVal;
//double tTime = 0;
//Timer_Start();
	for(i=0; i< sVariableVal.nValResult; i++){
		name = sVariableVal.ValName[i];
		if( sVariableVal.ptrDefVariableVal[i] != NULL ){
			valVariableVal = *(sVariableVal.ptrDefVariableVal[i]);
		}else{
			//�ް��ޯ̧��NULL�Ȃ̂�0���Ɠ��������ɂ���0�l�ɂ����
			valVariableVal = DBL_MAX;
		}
		//�ŐV�ް����ޯ̧����擾���ľ�Ă�����
		Calc.SetVariable(name, valVariableVal);
#ifdef DEBUG_PRINT2
		printf(">>>>>>>>>>> CalcByMemory::SetNewVariableDat()  name=%s(%f)\n", name.c_str(), valVariableVal);
#endif
	}
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("Calc.SetVariable()=%f\n", tTime);
//#endif
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetSelectDat
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Select���ް����
//----------------------------------------------------------------------------
void CalcByMemory::SetSelectDat(void)
{
	int i;
	int Offset;
	std::string strFlag;
	std::string strName;
	std::string strCalcName;
	char tmp[256];
	int valResult;
	double valCalcResult;

	for(i=0; i< sSelect.nSelectResult; i++){
		Offset  = sSelect.SelectOffset[i]; 
		strFlag = sSelect.SelectFlagName[i]; 
		strName = sSelect.SelectResultName[i];
		strCalcName = sSelect.CalcName[i]; 

		//�������͕ϐ��Ƃ��Ē�`
		valResult = (int)Calc.getSymbol(strFlag);
		sprintf(tmp, "%s.%d", strName.c_str(), (Offset + valResult) ); 
		valCalcResult = Calc.getSymbol(tmp);
		//Select�̌��ʂ�Ă���
		Calc.setSymbol(strCalcName, valCalcResult); 
	}
}

//ADD 2009-11-17
void CalcByMemory::SetSelectDat2(void)
{
	int i;
	int k;
	int Offset;
	std::string strFlag;
	std::string strName;
	std::string strCalcName;
	char tmp[256];
	int valResult;
	double valCalcResult;

	for(i=0; i< sSelect.nSelectResult; i++){
		Offset  = sSelect.SelectOffset[i]; 
		strFlag = sSelect.SelectFlagName[i]; 
		strName = sSelect.SelectResultName[i];
		strCalcName = sSelect.CalcName[i]; 

		//�������͎��Ƃ��Ē�`
		if( sSelect.SetExpressionFlag[i] > 0 ){
			for(k=0; k< sSelect.SelectItemCount[i]; k++){
				sprintf(tmp, "%s.%d", strName.c_str(), k+1 ); 
				valCalcResult = Calc.getSymbol(tmp);
				//���Z���ʂ�Ă���
				Calc.setSymbol(tmp, valCalcResult); 
			}
		}else{
			//�������͕ϐ��Ƃ��Ē�`
			valResult = (int)Calc.getSymbol(strFlag);
			sprintf(tmp, "%s.%d", strName.c_str(), (Offset + valResult) ); 
	
			valCalcResult = Calc.getSymbol(tmp);
			//Select�̌��ʂ�Ă���
			Calc.setSymbol(strCalcName, valCalcResult); 
		}
	}
}
//ADD 2009-11-17

//----------------------------------------------------------------------------
// FUNCTION NAME : GetSelectDat
//
// INPUT OUTPUT :
//		string		name				(I)	���Z����
//		string &	ResultName			(O)	EXCEL�`���ɕϊ��������Z��
//		int			(RETURN)			(R)	�߂�l(1�FSelect�֘A�A0�F���̑��̒ʏ�̎�)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Select�֘A�̎w�薼���牉�Z����W�J����
//----------------------------------------------------------------------------
int CalcByMemory::GetSelectDat(std::string name, std::string& ResultName)
{
	int i, k;
	int Offset;
	int SelectItemCount;
	std::string strFlag;
	std::string strName;
	std::string strCalcName;
	std::string tmp="";
	int val;
	char c[256];

	ResultName="";

	//"PHASE_DIST=IF(DIST_TYPE=0,PHASE_DIST_SEL.1,IF(DIST_TYPE=1,PHASE_DIST_SEL.2,0))"
	//"PHASE_DIST=IF(DIST_TYPE=0,PHASE_DIST_SEL.1,IF(DIST_TYPE=1,PHASE_DIST_SEL.2,IF(DIST_TYPE=2,PHASE_DIST_SEL.3,0)))"

	for(i=0; i< sSelect.nSelectResult; i++){
		strCalcName = sSelect.CalcName[i]; 
		Offset  = sSelect.SelectOffset[i]; 
		strFlag = sSelect.SelectFlagName[i]; 
		strName = sSelect.SelectResultName[i];
		SelectItemCount = sSelect.SelectItemCount[i];								//��@PHASE_DIST_SEL.1 ,PHASE_DIST_SEL.2 ,PHASE_DIST_SEL.3
		val = 0;
		if( strCalcName == name ){

			tmp = "";
			for(k=0; k< SelectItemCount; k++){
				if( k==0 ){
					sprintf(c, "IF(%s=%d,%s.%d", strFlag.c_str(), val, strName.c_str(), (k+1));
				}else{
					sprintf(c, ",IF(%s=%d,%s.%d", strFlag.c_str(), val, strName.c_str(), (k+1));
				}
				tmp = tmp + c ; 
				val++;
			}

			tmp = tmp + ",0";
			for(k=0; k< SelectItemCount; k++){
				tmp = tmp + ")";
			}

			ResultName = tmp;
			return 1;
		}
	}

	return 0;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetCalcResultDat
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		
//----------------------------------------------------------------------------
void CalcByMemory::SetCalcResultDat(void)
{
	int i;
	double valCalResult;
	for(i=0; i< sCalcResult.nCalcResult; i++){
		if( sCalcResult.CalcResultName[i].size() > 0 ){
			valCalResult = Calc.getSymbol(sCalcResult.CalcResultName[i]);				// ���Z���ʒl
			if( sCalcResult.ptrDefCalcResultVal[i] != NULL ){
				//�ޯ̧���ݒ肳��Ă����
				*sCalcResult.ptrDefCalcResultVal[i] = valCalResult;
			}
		}
	}
}

}// namespace Common
}// namespace OKD

