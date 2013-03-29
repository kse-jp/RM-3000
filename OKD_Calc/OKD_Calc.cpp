/////////////////////////////////////////////////////////////////////
//
//  OKD_Calc.cpp
//  演算DLLクラス
// 
//	2009-10-08	作成	中岡昌俊



#include "stdafx.h"
#include <cfloat>

#include "OKD_Calc.h"

////////////////////////////////////////////////////////////////////////////
#pragma warning( disable : 4996 )
#define _CRT_SECURE_NO_DEPRECATE

#ifdef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#undef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1
//strcopyやsprintf()などの(バッファオーバラン対策)ワーニングを抑止する
#endif
////////////////////////////////////////////////////////////////////////////

using namespace std ;

//#include	"CalcDef.h"
#include	"CalcEngine.h"
#include	"CalcRpn.h"

#include	"Tool.h"


static string		strRpn = "";	//RPNの文字列（式）を保持する
static string		strResult = "";

CalcEngine	Calc;
//static CalcRpn		CalcRpn;
CalcRpn		CalcRpn;

char ErrorString[ 512 ];
int		calcErrZero =0;	//DBL_MAX;


//演算結果用
struct	stCalcResult {
#define MAX_CALCVAL_NUM	2000
	int		nCalcResult;
	std::string CalcResultName[MAX_CALCVAL_NUM];
	double CalcResult[MAX_CALCVAL_NUM];
	double *ptrDefCalcResultVal[MAX_CALCVAL_NUM];
};
struct stCalcResult sCalcResult;

//変数値用
struct	stVariableVal {
#define MAX_VAL_NUM	2000
	int		nValResult;
	std::string ValName[MAX_VAL_NUM];
	double VariableVal[MAX_VAL_NUM];
	double *ptrDefVariableVal[MAX_VAL_NUM];
};
struct stVariableVal sVariableVal;

//Select演算用
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


// コンストラクタ
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

	//ｸﾘｱ
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
//		定数・変数・演算式設定情報の初期化
//----------------------------------------------------------------------------
void CalcByValue::Initialize()	// 戻り値なし
{
	//strRpn = "";
	//strResult = "";
	for( ; strRpn.size() > 0 ; ){
		strRpn.erase();
	}
	for( ; strResult.size() > 0 ; ){
		strResult.erase();
	}

	//ｸﾘｱ
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
//		管理ﾊﾞｯﾌｧのｸﾘｱ
//----------------------------------------------------------------------------
void CalcByValue::InitBuf(void)
{
	int i;
	//Select演算用
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
//		int			numVariable			(I)	変数の総数
//		string		strVariableName[]	(I)	変数名の配列
//		double *	ptrVariableVal[]	(I)	変数値の格納領域のポインタの配列
//		string &	strErrorMessage		(O)	エラー情報文字列（エラー内容・問題のある定数名など）
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変数名と変数値格納領域の設定
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

				//大文字変換
				Tool_ChgUpper( strVariableName[i], name);

				Calc.SetVariable(name, valVariableVal[i]); 
#ifdef DEBUG_PRINT1
			printf("name=%s. %f\n", name.c_str(), valVariableVal[i] );
#endif

				if( iSelectResult == 0 ){
					//Selectの確認		"PHASE_DIST_SEL.1";
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
							//代表名のみを登録
							sSelect.SelectResultName[sSelect.nSelectResult] = name2;		//PHASE_DIST_SEL
							sSelect.SelectItemCount[sSelect.nSelectResult]++;				//例　PHASE_DIST_SEL.1
							sSelect.nSelectResult++;

						}else{
							sSelect.SelectItemCount[iFind]++;								//例　PHASE_DIST_SEL.2 ,PHASE_DIST_SEL.3
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
//		int			numExpression		(I)	演算式の総数
//		string		strCalcName[]		(I)	演算式名の配列
//		string		strExpression[]		(I)	演算式の配列
//		string &	strErrorMessage		(O)	エラー情報文字列（エラー内容・問題のある定数名など）
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		演算式名と演算式と演算結果値格納領域の設定
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

    //式
	for( ; strRpn.size() > 0 ; ){
		strRpn.erase() ;
	}

	try {
		for(i=0; i<numExpression; i++){
			if( strCalcName[i].empty() != TRUE && strExpression[i].empty() != TRUE){

				//大文字変換
				Tool_ChgUpper( strCalcName[i], name);
				Tool_ChgUpper( strExpression[i], rpnname);


//ADD 2009-11-17
				//Selectの確認		"PHASE_DIST_SEL2.1";
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
						//代表名のみを登録
						sSelect.SelectResultName[sSelect.nSelectResult] = name2;		//PHASE_DIST_SEL2
						sSelect.SelectItemCount[sSelect.nSelectResult]++;				//例　PHASE_DIST_SEL2.1
						sSelect.SetExpressionFlag[sSelect.nSelectResult] = 1;
						sSelect.nSelectResult++;

					}else{
						sSelect.SelectItemCount[iFind]++;								//例　PHASE_DIST_SEL2.2 ,PHASE_DIST_SEL2.3
						sSelect.SetExpressionFlag[iFind] = 1;
					}
				}
//ADD 2009-11-17

				//Selectの確認
				if( ( k = rpnname.find( "SELECT" ) ) >= 0 ) {
					//見つかった

					//例　"SELECT(PHASE_DIST_SEL,DIST_TYPE,1)"  ----> "PHASE_DIST_SEL,DIST_TYPE,1"
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

					//ﾗﾍﾞﾙを登録
					Calc.SetVariable(name, 0.0F); 
					//式の生成
					sprintf(tmp, "%s=%s", name.c_str(), strExpression[i].c_str()); 
					//演算式を解読登録
					iErr = CalcRpn.strRPN( tmp, temp, sizeof(temp));
					if( iErr ){
						strErrorMessage = temp;
						iRet = rErrNG;
#ifdef DEBUG_PRINT1
						printf("\n\nError=%s\n", temp);
#endif
						break;
					}else{
						//ﾗﾍﾞﾙと式を登録
						Calc.SetRpn(name, strExpression[i]);
					}
#ifdef DEBUG_PRINT1
					printf("name=%s. %s\n  Calc変換後=%s\n", strCalcName[i].c_str(), strExpression[i].c_str(), temp);
#endif
					//変換済の式を登録
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
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		演算実行
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
	//Selectのﾃﾞｰﾀｾｯﾄ
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
	printf("切り出し間で=%f\n", tTime);
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
	//Selectのﾃﾞｰﾀｾｯﾄ
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
//		string		strCalcResultName	(I)	演算式名
//		string &	valCalResult		(O)	演算結果値
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		演算結果取得
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

	//指定ﾗﾍﾞﾙの演算結果を取得
	valCalResult = Calc.getSymbol(strCalcResultName);

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : GetExcelFormatExpression
//
// INPUT OUTPUT :
//		string		strCalcName			(I)	演算式名
//		string &	strExpression		(O)	EXCEL形式に変換した演算式名
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Excel用演算式取得
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

	//大文字変換
	Tool_ChgUpper( strCalcName, strName);
	if( GetSelectDat(strName, strTmp) ){
		//no error
		strExpression = strName + "=" + strTmp;
		return iRet;
	}

	//ﾗﾍﾞﾙと式を登録
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
//		Selectのﾃﾞｰﾀｾｯﾄ
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
		//Selectの結果をｾｯﾄする
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

		//こっちは式として定義
		if( sSelect.SetExpressionFlag[i] > 0 ){
			for(k=0; k< sSelect.SelectItemCount[i]; k++){
				sprintf(tmp, "%s.%d", strName.c_str(), k+1 ); 
				valCalcResult = Calc.getSymbol(tmp);
				//演算結果をｾｯﾄする
				Calc.setSymbol(tmp, valCalcResult); 
			}
		}else{
			//こっちは変数として定義
			valResult = (int)Calc.getSymbol(strFlag);
			sprintf(tmp, "%s.%d", strName.c_str(), (Offset + valResult) ); 
	
			valCalcResult = Calc.getSymbol(tmp);
			//Selectの結果をｾｯﾄする
			Calc.setSymbol(strCalcName, valCalcResult); 
		}
	}
}
//ADD 2009-11-17

//----------------------------------------------------------------------------
// FUNCTION NAME : GetSelectDat
//
// INPUT OUTPUT :
//		string		name				(I)	演算式名
//		string &	ResultName			(O)	EXCEL形式に変換した演算式
//		int			(RETURN)			(R)	戻り値(1：Select関連、0：その他の通常の式)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Select関連の指定名から演算式を展開する
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
		SelectItemCount = sSelect.SelectItemCount[i];								//例　PHASE_DIST_SEL.1 ,PHASE_DIST_SEL.2 ,PHASE_DIST_SEL.3
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


// コンストラクタ
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

	//ｸﾘｱ
	InitBuf();
	Calc.init(); 

	return;
}

// デストラクタ
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
//		管理ﾊﾞｯﾌｧのｸﾘｱ
//----------------------------------------------------------------------------
void CalcByMemory::InitBuf(void)
{
	int i;
	//演算結果用
	for(i=0; i<MAX_CALCVAL_NUM; i++){
		sCalcResult.CalcResultName[i] = "";
		//演算結果値の格納領域のポインタの配列
		sCalcResult.ptrDefCalcResultVal[i] = &sCalcResult.CalcResult[i];
	}
	sCalcResult.nCalcResult = 0;


	//変数値用
	for(i=0; i<MAX_VAL_NUM; i++){
		sVariableVal.ValName[i] = "";
		//演算結果値の格納領域のポインタの配列
		sVariableVal.ptrDefVariableVal[i] = &sVariableVal.VariableVal[i];
	}
	sVariableVal.nValResult = 0;

	//Select演算用
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
//		定数・変数・演算式設定情報の初期化
//----------------------------------------------------------------------------
void CalcByMemory::Initialize()			// 戻り値なし
{

	//strRpn = "";
	//strResult = "";
	for( ; strRpn.size() > 0 ; ){
		strRpn.erase();
	}
	for( ; strResult.size() > 0 ; ){
		strResult.erase();
	}

	//ｸﾘｱ
	InitBuf();
	Calc.init(); 

	return;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetConstant
//
// INPUT OUTPUT :
//		int			numConstant			(I)	定数の総数
//		string		strConstantName[]	(I)	定数名の配列
//		double		valConstant[]		(I)	定数値の配列
//		string &	strErrorMessage		(O)	エラー情報文字列（エラー内容・問題のある定数名など）
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		定数名と定数値の設定
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
//		int			numVariable			(I)	変数の総数
//		string		strVariableName[]	(I)	変数名の配列
//		double *	ptrVariableVal[]	(I)	変数値の格納領域のポインタの配列
//		string &	strErrorMessage		(O)	エラー情報文字列（エラー内容・問題のある定数名など）
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変数名と変数値格納領域の設定
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

				//大文字変換
				Tool_ChgUpper( strVariableName[i], name);

				if( ptrVariableVal[i] != NULL ){
					Calc.SetVariable(name, *(ptrVariableVal[i])); 
#ifdef DEBUG_PRINT1
					printf("name=%s. %f\n", name.c_str(), *(ptrVariableVal[i]) );
#endif
					//演算結果値の格納領域のポインタの配列
					sVariableVal.ptrDefVariableVal[i] = ptrVariableVal[i];

//debug				*(sVariableVal.ptrDefVariableVal[i])=i+1+100;

				}else{
					//ﾃﾞｰﾀﾊﾞｯﾌｧがNULLなので
					Calc.SetVariable(name, 0.0); 
					sVariableVal.ptrDefVariableVal[i] = NULL;
				}

				sVariableVal.ValName[i] = name;
				sVariableVal.nValResult++;

				//Selectの確認		"PHASE_DIST_SEL.1";
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
						//代表名のみを登録
						sSelect.SelectResultName[sSelect.nSelectResult] = name2;		//PHASE_DIST_SEL
						sSelect.SelectItemCount[sSelect.nSelectResult]++;				//例　PHASE_DIST_SEL.1
						sSelect.nSelectResult++;

					}else{
						sSelect.SelectItemCount[iFind]++;								//例　PHASE_DIST_SEL.2 ,PHASE_DIST_SEL.3
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
//		int			numExpression		(I)	演算式の総数
//		string		strCalcName[]		(I)	演算式名の配列
//		string		strExpression[]		(I)	演算式の配列
//		double *	ptrCalcResult[]		(I)	演算結果値の格納領域のポインタの配列（省略可能）
//		string &	strErrorMessage		(O)	エラー情報文字列（エラー内容・問題のある定数名など）
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		演算式名と演算式と演算結果値格納領域の設定
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

    //式
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
				//大文字変換
				Tool_ChgUpper( strCalcName[i], name);
				Tool_ChgUpper( strExpression[i], rpnname);

//ADD 2009-11-17
				//Selectの確認		"PHASE_DIST_SEL2.1";
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
						//代表名のみを登録
						sSelect.SelectResultName[sSelect.nSelectResult] = name2;		//PHASE_DIST_SEL2
						sSelect.SelectItemCount[sSelect.nSelectResult]++;				//例　PHASE_DIST_SEL2.1
						sSelect.SetExpressionFlag[sSelect.nSelectResult] = 1;
						sSelect.nSelectResult++;

					}else{
						sSelect.SelectItemCount[iFind]++;								//例　PHASE_DIST_SEL2.2 ,PHASE_DIST_SEL2.3
						sSelect.SetExpressionFlag[iFind] = 1;
					}
				}
//ADD 2009-11-17

				//Selectの確認
				if( ( k = rpnname.find( "SELECT" ) ) >= 0 ) {
					//見つかった

					//例　"SELECT(PHASE_DIST_SEL,DIST_TYPE,1)"  ----> "PHASE_DIST_SEL,DIST_TYPE,1"
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
					//ﾗﾍﾞﾙを登録
					Calc.SetVariable(name, 0.0F); 
					//式の生成
					sprintf(tmp, "%s=%s", name.c_str(), strExpression[i].c_str()); 
					//演算式を解読登録
					iErr = CalcRpn.strRPN( tmp, temp, sizeof(temp));
					if( iErr ){
						strErrorMessage = temp;
						iRet = rErrNG;
#ifdef DEBUG_PRINT1
						printf("\n\nError=%s\n", temp);
#endif
						break;
					}else{
						//ﾗﾍﾞﾙと式を登録
						Calc.SetRpn(name, strExpression[i]);
					}
#ifdef DEBUG_PRINT1
					printf("name=%s. %s\n  Calc変換後=%s\n", name.c_str(), strExpression[i].c_str(), temp);
#endif
					//変換済の式を登録
					strRpn = strRpn + temp +"\r\n" ;
				}
				
				//演算結果値の格納領域のポインタの配列
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
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		演算実行
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
	//上位のﾃﾞｰﾀ用のﾊﾞｯﾌｧから最新を内部をｾｯﾄし直す
	SetNewVariableDat();

//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("0Time=%f\n", tTime);
//#endif

	///////////////////////////////////////////////
	//Selectのﾃﾞｰﾀｾｯﾄ
	SetSelectDat();

//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	//printf("式=%s\n", strRpn.c_str());
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
	printf("切り出しまで=%f\n", tTime);
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
			//printf("Calc.rpnOperation()で=%s\n", temp);
//Timer_Stop(&tTime);
//tTimeAdd += tTime;
//#ifdef DEBUG_PRINT5
//	printf("Calc.rpnOperation()で=%f\n", tTime);
//#endif
		}
	}
//#ifdef DEBUG_PRINT5
//	printf("Calc.rpnOperation()　完了=%f\n", tTimeAdd);
//#endif

//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("Calc.rpnOperation()で=%f\n", tTime);
//#endif

//ADD 2009-11-17
	///////////////////////////////////////////////
	//Selectのﾃﾞｰﾀｾｯﾄ
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
	//演算結果値の格納領域のﾎﾟｲﾝﾀ配列にｾｯﾄする
	SetCalcResultDat();
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("ﾎﾟｲﾝﾀ配列にｾｯﾄで=%f\n", tTime);
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
//		string		strCalcResultName	(I)	演算式名
//		string &	valCalResult		(O)	演算結果値
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		演算結果取得
//----------------------------------------------------------------------------
int CalcByMemory::GetCalcResult(		// 戻り値(0：成功、1：失敗)
		std::string strCalcResultName,		// 演算式名
		double &valCalResult)				// 演算結果値
{
	int iRet = rErrOK;

	if( strCalcResultName.empty() == TRUE){
		iRet = rErrNG;
		return iRet;
	}

	//指定ﾗﾍﾞﾙの演算結果を取得
	valCalResult = Calc.getSymbol(strCalcResultName);

	return iRet;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : GetExcelFormatExpression
//
// INPUT OUTPUT :
//		string		strCalcName			(I)	演算式名
//		string &	strExpression		(O)	EXCEL形式に変換した演算式名
//		int			(RETURN)			(R)	戻り値(0：成功、1：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Excel用演算式取得
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

	//大文字変換
	Tool_ChgUpper( strCalcName, strName);
	if( GetSelectDat(strName, strTmp) ){
		//no error
		strExpression = strName + "=" + strTmp;
		return iRet;
	}

	//ﾗﾍﾞﾙと式を登録
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
//		上位のﾃﾞｰﾀ用のﾊﾞｯﾌｧから最新を内部をｾｯﾄし直す
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
			//ﾃﾞｰﾀﾊﾞｯﾌｧがNULLなので0割と同じ処理にして0値にする為
			valVariableVal = DBL_MAX;
		}
		//最新ﾃﾞｰﾀをﾊﾞｯﾌｧから取得してｾｯﾄし直す
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
//		Selectのﾃﾞｰﾀｾｯﾄ
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

		//こっちは変数として定義
		valResult = (int)Calc.getSymbol(strFlag);
		sprintf(tmp, "%s.%d", strName.c_str(), (Offset + valResult) ); 
		valCalcResult = Calc.getSymbol(tmp);
		//Selectの結果をｾｯﾄする
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

		//こっちは式として定義
		if( sSelect.SetExpressionFlag[i] > 0 ){
			for(k=0; k< sSelect.SelectItemCount[i]; k++){
				sprintf(tmp, "%s.%d", strName.c_str(), k+1 ); 
				valCalcResult = Calc.getSymbol(tmp);
				//演算結果をｾｯﾄする
				Calc.setSymbol(tmp, valCalcResult); 
			}
		}else{
			//こっちは変数として定義
			valResult = (int)Calc.getSymbol(strFlag);
			sprintf(tmp, "%s.%d", strName.c_str(), (Offset + valResult) ); 
	
			valCalcResult = Calc.getSymbol(tmp);
			//Selectの結果をｾｯﾄする
			Calc.setSymbol(strCalcName, valCalcResult); 
		}
	}
}
//ADD 2009-11-17

//----------------------------------------------------------------------------
// FUNCTION NAME : GetSelectDat
//
// INPUT OUTPUT :
//		string		name				(I)	演算式名
//		string &	ResultName			(O)	EXCEL形式に変換した演算式
//		int			(RETURN)			(R)	戻り値(1：Select関連、0：その他の通常の式)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		Select関連の指定名から演算式を展開する
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
		SelectItemCount = sSelect.SelectItemCount[i];								//例　PHASE_DIST_SEL.1 ,PHASE_DIST_SEL.2 ,PHASE_DIST_SEL.3
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
			valCalResult = Calc.getSymbol(sCalcResult.CalcResultName[i]);				// 演算結果値
			if( sCalcResult.ptrDefCalcResultVal[i] != NULL ){
				//ﾊﾞｯﾌｧが設定されていれば
				*sCalcResult.ptrDefCalcResultVal[i] = valCalResult;
			}
		}
	}
}

}// namespace Common
}// namespace OKD

