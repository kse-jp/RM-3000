// ����� ���C�� DLL �t�@�C���ł��B

#include "stdafx.h"

#include "msclr/marshal_cppstd.h"

#include "OKD_Calc_Wrapper.h"
//#pragma comment( lib, "../Output/lib/OKD_Calc.lib")
#pragma comment( lib, "../bin/Release/OKD_Calc.lib")

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;
using namespace System::Reflection;


using namespace msclr::interop;


namespace OKD
{


namespace Common
{

#pragma region CalcByValue_Wrapper
	
	//----------------------------------------------------------------------------
	// FUNCTION NAME : Constructor
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//----------------------------------------------------------------------------
	CalcByValue_Wrapper::CalcByValue_Wrapper()
	{
		calc = new OKD::Common::CalcByValue();
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : Destructor
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//----------------------------------------------------------------------------
	CalcByValue_Wrapper::~CalcByValue_Wrapper()
	{
		delete calc;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : Initialize
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//----------------------------------------------------------------------------
	void CalcByValue_Wrapper::Initialize()
	{
		calc->Initialize();
	}
	
	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//----------------------------------------------------------------------------
	int CalcByValue_Wrapper::SetVariable(
		int numVariable, 
		array<String ^>^ strVariableName, 
		array<double>^ valVariableVal, 
		String^% strErrorMessage
		)
	{
		
		//Variable List Casted array<System::String^>^ to std::string* 
		std::string* lststrVariableName = new std::string[strVariableName->Length];
		ToStdStrArray(strVariableName, lststrVariableName);

		//Variable Value List Pointer Pinned
		pin_ptr<double> lstvalVariableVal = &valVariableVal[0];
		
		//Define Result Area
		std::string strErrorMessage_Res;

		//Called
		int ret = calc->SetVariable(numVariable, lststrVariableName, lstvalVariableVal , strErrorMessage_Res);

		//Resule Casted std::string to System::String^ 
		strErrorMessage = ToCLI(strErrorMessage_Res, System::Text::Encoding::GetEncoding("shift-jis"));

		//Deleted
		delete[] lststrVariableName;

		//pin_ptr -> nullptr
		lstvalVariableVal = nullptr;

		return ret;

	}


	//----------------------------------------------------------------------------
	// FUNCTION NAME : SetExpression 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// ���Z�����Ɖ��Z���Ɖ��Z���ʒl�i�[�̈�̐ݒ�
	//----------------------------------------------------------------------------
	int CalcByValue_Wrapper::SetExpression(				// �߂�l(0�F�����A1�F���s)
				int numExpression,					// ���Z���̑���
				array<String^>^ strCalcName,			// ���Z����
				array<String^>^ strExpression,		// ���Z���̔z��
				String^% strErrorMessage)			// �G���[��񕶎���i�G���[���e�E���̂��鉉�Z�����E���Z�����e�Ȃǁj���p���Ƃ���
	{
		//Calculator Name List Casted array<System::String^>^ to std::string* 
		std::string* lststrCalcName = new std::string[strCalcName->Length];
		ToStdStrArray(strCalcName, lststrCalcName);

		//Expression List Casted array<System::String^>^ to std::string* 
		std::string* lststrExpression = new std::string[strExpression->Length];
		ToStdStrArray(strExpression, lststrExpression);
	
		//Define Result Area
		std::string strErrorMessage_Res;

		//Called
		int ret = calc->SetExpression(numExpression, lststrCalcName, lststrExpression , strErrorMessage_Res);

		//Resule Casted std::string to System::String^ 
		strErrorMessage = ToCLI(strErrorMessage_Res, System::Text::Encoding::GetEncoding("shift-jis"));

		//Deleted
		delete[] lststrCalcName;
		delete[] lststrExpression;

		return ret;
	}
	//----------------------------------------------------------------------------
	// FUNCTION NAME : Execute 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// ���Z���s
	//----------------------------------------------------------------------------
	int CalcByValue_Wrapper::Execute(					// �߂�l(0�F�����A1�F���s)
		String^% strErrorMessage)						// �G���[��񕶎��񁦉p���Ƃ���
	{
		//Define Result Area
		std::string strErrorMessage_Res;

		//Called
		int ret = calc->Execute(strErrorMessage_Res);

		//Resule Casted std::string to System::String^ 
		strErrorMessage = ToCLI(strErrorMessage_Res, System::Text::Encoding::GetEncoding("shift-jis"));

		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : GetCalcResult 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// ���Z���ʎ擾
	//----------------------------------------------------------------------------
	int CalcByValue_Wrapper::GetCalcResult(				// �߂�l(0�F�����A1�F���s)
		String^ strCalcResultName,			// ���Z����
		double% valCalResult)				// ���Z���ʒl
	{

		//CalcResultName Casted System::String^ to std::string
		std::string pstrCalcResultName = FromCLI(strCalcResultName, System::Text::Encoding::GetEncoding("shift-jis"));

		//Define Result Area
		double valCalResult_Res;

		//Called
		int ret = calc->GetCalcResult(pstrCalcResultName , valCalResult_Res);

		//Resule
		valCalResult = valCalResult_Res;

		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : GetCalcResult 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// Excel�p���Z���擾
	//----------------------------------------------------------------------------
	int CalcByValue_Wrapper::GetExcelFormatExpression(	// �߂�l(0�F�����A1�F���s)
		String^ strCalcName,				// ���Z����
		String^ %strExpression)				// EXCEL�`���ɕϊ��������Z����
	{

		//CalcName Casted System::String^ to std::string
		std::string pstrCalcName = FromCLI(strCalcName, System::Text::Encoding::GetEncoding("shift-jis"));

		//Define Result Area
		std::string strExpression_Res;

		//Called
		int ret = calc->GetExcelFormatExpression(pstrCalcName, strExpression_Res);

		//Resule Casted std::string to System::String^ 
		strExpression = ToCLI(strExpression_Res, System::Text::Encoding::GetEncoding("shift-jis"));

		return ret;
	}

#pragma endregion 

#pragma region CalcByMemory_Wrapper

	//----------------------------------------------------------------------------
	// FUNCTION NAME : Constructor
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//----------------------------------------------------------------------------
	CalcByMemory_Wrapper::CalcByMemory_Wrapper()
	{
		calc = new OKD::Common::CalcByMemory();
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : Destructor
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//----------------------------------------------------------------------------
	CalcByMemory_Wrapper::~CalcByMemory_Wrapper()
	{
		delete calc;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : Initialize
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// �萔�E�ϐ��E���Z���ݒ���̏�����
	//----------------------------------------------------------------------------
	void CalcByMemory_Wrapper::Initialize()				// �߂�l�Ȃ�
	{
		calc->Initialize();
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : SetConstant
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// �萔���ƒ萔�l�̐ݒ�
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::SetConstant(				// �߂�l(0�F�����A1�F���s)
		int numConstant,					// �萔�̑���
		array<String^>^ strConstantName,// �萔���̔z��
		array<double>^ valConstant,			// �萔�l�̔z��
		String^% strErrorMessage)			// �G���[��񕶎���i�G���[���e�E���̂���萔���Ȃǁj���p���Ƃ���
	{
		//Constant Name List Casted array<System::String^>^ to std::string* 
		std::string* lststrConstantName = new std::string[strConstantName->Length];
		ToStdStrArray(strConstantName, lststrConstantName);

		//Constant Value List Pointer Pinned
		pin_ptr<double> lstvalConstant = &valConstant[0];
		
		//Define Result Area
		std::string strErrorMessage_Res;

		//Called
		int ret = calc->SetConstant(numConstant, lststrConstantName, lstvalConstant , strErrorMessage_Res);

		//Resule Casted std::string to System::String^ 
		strErrorMessage = ToCLI(strErrorMessage_Res, System::Text::Encoding::GetEncoding("shift-jis"));

		//Deleted
		delete[] lststrConstantName;

		//pin_ptr -> nullptr
		lstvalConstant = nullptr;

		return ret;
	}
	//----------------------------------------------------------------------------
	// FUNCTION NAME : SetVariable
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// �ϐ����ƕϐ��l�i�[�̈�̐ݒ�
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::SetVariable(				// �߂�l(0�F�����A1�F���s)
		int numVariable,					// �ϐ��̑���
		array<String^>^ strVariableName,		// �ϐ����̔z��
		//System::Collections::ArrayList^% ptrVariableVal,		// �ϐ��l�̊i�[�̈�̃|�C���^�̔z��
		array<double> ^ptrVariableVal,		// �ϐ��l�̊i�[�̈�̃|�C���^�̔z��
	    String^% strErrorMessage)			// �G���[��񕶎���i�G���[���e�E���̂���ϐ����Ȃǁj���p���Ƃ���
	{
		//Variable List Casted array<System::String^>^ to std::string* 
		std::string* lststrVariableName = new std::string[strVariableName->Length];
		ToStdStrArray(strVariableName, lststrVariableName);

		//Make Variable Pointor List
		//double** lstptrVariableVal = new double* [ptrVariableVal->Count];
		double** lstptrVariableVal = new double* [strVariableName->Length];
		
		for(int i = 0 ; i < strVariableName->Length ; i++)
		{
			//System::Double^ dh = (System::Double^)ptrVariableVal[i];
			//pin_ptr<double> ptr = &(double)dh;
		    pin_ptr<double> g_ptr = &ptrVariableVal[i]; 
			*(lstptrVariableVal + i) = g_ptr;
		}
		
		//Define Result Area
		std::string strErrorMessage_Res;

		//Called
		int ret = calc->SetVariable(numVariable, lststrVariableName, lstptrVariableVal , strErrorMessage_Res);

		//Resule Casted std::string to System::String^ 
		strErrorMessage = ToCLI(strErrorMessage_Res, System::Text::Encoding::GetEncoding("shift-jis"));

		//pin_ptr -> nullptr
		for(int i = 0 ; i < strVariableName->Length ; i++)
		{
//printf("C++/CLI SetVariable(%d)=%f\n", i, **(lstptrVariableVal + i));
			lstptrVariableVal[i] = nullptr;
			if( i == 6 ){	break;	}
		}

		//Deleted
		delete[] lststrVariableName;
		delete[] lstptrVariableVal;

		return ret;
	}
	//----------------------------------------------------------------------------
	// FUNCTION NAME : SetExpression
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// ���Z�����Ɖ��Z���Ɖ��Z���ʒl�i�[�̈�̐ݒ�
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::SetExpression(				// �߂�l(0�F�����A1�F���s)
		int numExpression,					// ���Z���̑���
		array<String^>^ strCalcName,			// ���Z����
		array<String^>^ strExpression,		// ���Z���̔z��
		//System::Collections::ArrayList^% ptrCalcResultVal,	// ���Z���ʒl�̊i�[�̈�̃|�C���^�̔z��i�ȗ��\�j
		array<double> ^ptrCalcResultVal,	// �ϐ��l�̊i�[�̈�̃|�C���^�̔z��
		String^% strErrorMessage)		    // �G���[��񕶎���i�G���[���e�E���̂��鉉�Z�����E���Z�����e�Ȃǁj���p���Ƃ���
	{
		//Calculator Name List Casted array<System::String^>^ to std::string* 
		std::string* lststrCalcName = new std::string[strCalcName->Length];
		ToStdStrArray(strCalcName,lststrCalcName);

		//Expression List Casted array<System::String^>^ to std::string* 
		std::string* lststrExpression = new std::string[strExpression->Length];
		ToStdStrArray(strExpression, lststrExpression);


		//Make Calc Pointor List
		//double** lstptrCalcResultVal = new double* [ptrCalcResultVal->Count];
		double** lstptrCalcResultVal = new double* [strCalcName->Length];
		
		for(int i = 0 ; i < strCalcName->Length ; i++)
		{
			//System::Double^ dh = (System::Double^)ptrCalcResultVal[i];
			//pin_ptr<double> ptr = &(double)dh;
		    pin_ptr<double> g_ptr2 = &ptrCalcResultVal[i]; 
			*(lstptrCalcResultVal + i) = g_ptr2;
		}


		//Define Result Area
		std::string strErrorMessage_Res;

		//Called
		int ret = calc->SetExpression(numExpression, lststrCalcName, lststrExpression ,lstptrCalcResultVal , strErrorMessage_Res);

		//Resule Casted std::string to System::String^ 
		strErrorMessage = ToCLI(strErrorMessage_Res, System::Text::Encoding::GetEncoding("shift-jis"));

		//pin_ptr -> nullptr
		for(int i = 0 ; i <strCalcName->Length ; i++)
		{
//printf("C++/CLI SetExpression(%d)=%f\n", i, **(lstptrCalcResultVal + i));
			lstptrCalcResultVal[i] = nullptr;
			if( i == 6 ){	break;	}
		}

		//Deleted
		delete[] lststrCalcName;
		delete[] lststrExpression;
		delete[] lstptrCalcResultVal;


		return ret;
	}
	//----------------------------------------------------------------------------
	// FUNCTION NAME : Execute
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// ���Z���s
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::Execute(					// �߂�l(0�F�����A1�F���s)
				String^% strErrorMessage)			// �G���[��񕶎��񁦉p���Ƃ���
	{
		//Define Result Area
		std::string strErrorMessage_Res;

		//Called
		int ret = calc->Execute(strErrorMessage_Res);

		//Resule Casted std::string to System::String^ 
		strErrorMessage = ToCLI(strErrorMessage_Res, System::Text::Encoding::GetEncoding("shift-jis"));

		return ret;
	}
	//----------------------------------------------------------------------------
	// FUNCTION NAME : GetCalcResult
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// ���Z���ʎ擾
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::GetCalcResult(				// �߂�l(0�F�����A1�F���s)
				String^ strCalcResultName,	// ���Z����
				double% valCalResult)				// ���Z���ʒl
	{
		//CalcResultName Casted System::String^ to std::string
		std::string pstrCalcResultName = FromCLI(strCalcResultName, System::Text::Encoding::GetEncoding("shift-jis"));

		//Define Result Area
		double valCalResult_Res;

		//Called
		int ret = calc->GetCalcResult(pstrCalcResultName , valCalResult_Res);

		//Resule
		valCalResult = valCalResult_Res;

		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : GetCalcResult
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// Excel�p���Z���擾
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::GetExcelFormatExpression(	// �߂�l(0�F�����A1�F���s)
				String^ strCalcName,				// ���Z����
				String^% strExpression)				// EXCEL�`���ɕϊ��������Z����
	{
		//CalcName Casted System::String^ to std::string
		std::string pstrCalcName = FromCLI(strCalcName, System::Text::Encoding::GetEncoding("shift-jis"));

		//Define Result Area
		std::string strExpression_Res;

		//Called
		int ret = calc->GetExcelFormatExpression(pstrCalcName, strExpression_Res);

		//Resule Casted std::string to System::String^ 
		strExpression = ToCLI(strExpression_Res, System::Text::Encoding::GetEncoding("shift-jis"));

		return ret;
	}


#pragma endregion

} // namespace Common

} // namespace OKD