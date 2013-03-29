// これは メイン DLL ファイルです。

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
	// 演算式名と演算式と演算結果値格納領域の設定
	//----------------------------------------------------------------------------
	int CalcByValue_Wrapper::SetExpression(				// 戻り値(0：成功、1：失敗)
				int numExpression,					// 演算式の総数
				array<String^>^ strCalcName,			// 演算式名
				array<String^>^ strExpression,		// 演算式の配列
				String^% strErrorMessage)			// エラー情報文字列（エラー内容・問題のある演算式名・演算式内容など）※英文とする
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
	// 演算実行
	//----------------------------------------------------------------------------
	int CalcByValue_Wrapper::Execute(					// 戻り値(0：成功、1：失敗)
		String^% strErrorMessage)						// エラー情報文字列※英文とする
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
	// 演算結果取得
	//----------------------------------------------------------------------------
	int CalcByValue_Wrapper::GetCalcResult(				// 戻り値(0：成功、1：失敗)
		String^ strCalcResultName,			// 演算式名
		double% valCalResult)				// 演算結果値
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
	// Excel用演算式取得
	//----------------------------------------------------------------------------
	int CalcByValue_Wrapper::GetExcelFormatExpression(	// 戻り値(0：成功、1：失敗)
		String^ strCalcName,				// 演算式名
		String^ %strExpression)				// EXCEL形式に変換した演算式名
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
	// 定数・変数・演算式設定情報の初期化
	//----------------------------------------------------------------------------
	void CalcByMemory_Wrapper::Initialize()				// 戻り値なし
	{
		calc->Initialize();
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : SetConstant
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	// 定数名と定数値の設定
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::SetConstant(				// 戻り値(0：成功、1：失敗)
		int numConstant,					// 定数の総数
		array<String^>^ strConstantName,// 定数名の配列
		array<double>^ valConstant,			// 定数値の配列
		String^% strErrorMessage)			// エラー情報文字列（エラー内容・問題のある定数名など）※英文とする
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
	// 変数名と変数値格納領域の設定
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::SetVariable(				// 戻り値(0：成功、1：失敗)
		int numVariable,					// 変数の総数
		array<String^>^ strVariableName,		// 変数名の配列
		//System::Collections::ArrayList^% ptrVariableVal,		// 変数値の格納領域のポインタの配列
		array<double> ^ptrVariableVal,		// 変数値の格納領域のポインタの配列
	    String^% strErrorMessage)			// エラー情報文字列（エラー内容・問題のある変数名など）※英文とする
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
	// 演算式名と演算式と演算結果値格納領域の設定
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::SetExpression(				// 戻り値(0：成功、1：失敗)
		int numExpression,					// 演算式の総数
		array<String^>^ strCalcName,			// 演算式名
		array<String^>^ strExpression,		// 演算式の配列
		//System::Collections::ArrayList^% ptrCalcResultVal,	// 演算結果値の格納領域のポインタの配列（省略可能）
		array<double> ^ptrCalcResultVal,	// 変数値の格納領域のポインタの配列
		String^% strErrorMessage)		    // エラー情報文字列（エラー内容・問題のある演算式名・演算式内容など）※英文とする
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
	// 演算実行
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::Execute(					// 戻り値(0：成功、1：失敗)
				String^% strErrorMessage)			// エラー情報文字列※英文とする
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
	// 演算結果取得
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::GetCalcResult(				// 戻り値(0：成功、1：失敗)
				String^ strCalcResultName,	// 演算式名
				double% valCalResult)				// 演算結果値
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
	// Excel用演算式取得
	//----------------------------------------------------------------------------
	int CalcByMemory_Wrapper::GetExcelFormatExpression(	// 戻り値(0：成功、1：失敗)
				String^ strCalcName,				// 演算式名
				String^% strExpression)				// EXCEL形式に変換した演算式名
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