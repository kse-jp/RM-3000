// OKD_Calc_Wrapper.h

#pragma once

#include "OKD_Calc.h"

#pragma comment( lib, "OKD_Calc.lib")

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;

namespace OKD
{

namespace Common
{

	#pragma region CalcByValue_Wrapper

	public ref class CalcByValue_Wrapper {
		public:
			// コンストラクタ
			CalcByValue_Wrapper();
			// デストラクタ
			~CalcByValue_Wrapper();

			// 変数・演算式設定情報の初期化
			virtual void Initialize();				// 戻り値なし

			// 変数名と変数値の設定
			virtual int SetVariable(				// 戻り値(0：成功、1：失敗)
				int numVariable,					// 変数の総数
				array<String^>^ strVariableName,	// 変数名の配列
				array<double>^ valVariableVal,		// 変数値の配列
				String^% strErrorMessage);			// エラー情報文字列（エラー内容・問題のある変数名など）※英文とする

			// 演算式名と演算式と演算結果値格納領域の設定
			virtual int SetExpression(				// 戻り値(0：成功、1：失敗)
				int numExpression,					// 演算式の総数
				array<String^>^ strCalcName,		// 演算式名
				array<String^>^ strExpression,		// 演算式の配列
				String^% strErrorMessage);			// エラー情報文字列（エラー内容・問題のある演算式名・演算式内容など）※英文とする

			// 演算実行
			virtual int Execute(					// 戻り値(0：成功、1：失敗)
				String^% strErrorMessage);			// エラー情報文字列※英文とする
			
			// 演算結果取得
			virtual int GetCalcResult(				// 戻り値(0：成功、1：失敗)
				String^ strCalcResultName,			// 演算式名
				double% valCalResult);				// 演算結果値

			// Excel用演算式取得
			virtual int GetExcelFormatExpression(	// 戻り値(0：成功、1：失敗)
				String^ strCalcName,				// 演算式名
				String^% strExpression);			// EXCEL形式に変換した演算式名

		private:
			OKD::Common::CalcByValue* calc;
	
	};
	#pragma endregion 

	#pragma region CalcByMemory_Wrapper


	////////////////////////////////////////////////////////////////////////////////////////////////

	public ref class CalcByMemory_Wrapper
	{
		public:
			// コンストラクタ
			CalcByMemory_Wrapper();

			// デストラクタ
			~CalcByMemory_Wrapper();

			// 定数・変数・演算式設定情報の初期化
			virtual void Initialize();				// 戻り値なし

			// 定数名と定数値の設定
			virtual int SetConstant(				// 戻り値(0：成功、1：失敗)
				int numConstant,					// 定数の総数
				array<String^>^ strConstantName,	// 定数名の配列
				array<double>^ valConstant,			// 定数値の配列
				String^% strErrorMessage);			// エラー情報文字列（エラー内容・問題のある定数名など）※英文とする

			// 変数名と変数値格納領域の設定
			virtual int SetVariable(				// 戻り値(0：成功、1：失敗)
				int numVariable,					// 変数の総数
				array<String^>^ strVariableName,	// 変数名の配列
				array<double> ^ptrVariableVal,		// 変数値の格納領域のポインタの配列
			    String^% strErrorMessage);			// エラー情報文字列（エラー内容・問題のある変数名など）※英文とする

			// 演算式名と演算式と演算結果値格納領域の設定
			virtual int SetExpression(				// 戻り値(0：成功、1：失敗)
				int numExpression,					// 演算式の総数
				array<String^>^ strCalcName,		// 演算式名
				array<String^>^ strExpression,		// 演算式の配列
				array<double> ^ptrCalcResultVal,	// 変数値の格納領域のポインタの配列
				String^% strErrorMessage);		    // エラー情報文字列（エラー内容・問題のある演算式名・演算式内容など）※英文とする

			// 演算実行
			virtual int Execute(					// 戻り値(0：成功、1：失敗)
				String^% strErrorMessage);			// エラー情報文字列※英文とする

			// 演算結果取得
			virtual int GetCalcResult(				// 戻り値(0：成功、1：失敗)
				String^ strCalcResultName,			// 演算式名
				double% valCalResult);				// 演算結果値

			// Excel用演算式取得
			virtual int GetExcelFormatExpression(	// 戻り値(0：成功、1：失敗)
				String^ strCalcName,				// 演算式名
				String^% strExpression);			// EXCEL形式に変換した演算式名

		private:
			OKD::Common::CalcByMemory* calc;

	};
	#pragma endregion



	// std::stringからSystem::Stringへの変換
	System::String^ ToCLI(
	  std::string& input,
	  System::Text::Encoding^ encoding)
	{
	  return gcnew System::String(
		input.data(), 0, input.size(), encoding);
	}; 

	// System::Stringからstd::stringへの変換
	std::string FromCLI(
	  String^ input,
	  System::Text::Encoding^ encoding) {
	  std::string result;
	  if ( input != nullptr &&  input->Length > 0 ) {
		array<Byte>^ barray =
		  System::Text::Encoding::Convert(
			  System::Text::Encoding::Unicode, // 変換元エンコーディング
			  encoding,          // 変換先エンコーディング
			  System::Text::Encoding::Unicode->GetBytes(input));
		pin_ptr<Byte> pin = &barray[0];
		result.assign(reinterpret_cast<char*>(pin), barray->Length);
	  }
	  return result;
	};  
 

	void ToStdStrArray(array<System::String^>^ Src, std::string* Dest)
	{		
		for(int i = 0; i < Src->Length ; i++)
		{
			Dest[i] = FromCLI(Src[i], Text::Encoding::GetEncoding("shift-jis"));
		}

	}


} // namespace Common

} // namespace OKD