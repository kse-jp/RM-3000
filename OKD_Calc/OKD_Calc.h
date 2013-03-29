/////////////////////////////////////////////////////////////////////
//
//  OKD_Calc.h
//  演算DLLクラス
// 
//	2009-10-08	作成	中岡昌俊


// 以下の ifdef ブロックは DLL からのエクスポートを容易にするマクロを作成するための 
// 一般的な方法です。この DLL 内のすべてのファイルは、コマンド ラインで定義された OKD_CALC_EXPORTS
// シンボルでコンパイルされます。このシンボルは、この DLL を使うプロジェクトで定義することはできません。
// ソースファイルがこのファイルを含んでいる他のプロジェクトは、 
// OKD_Calc_API 関数を DLL からインポートされたと見なすのに対し、この DLL は、このマクロで定義された
// シンボルをエクスポートされたと見なします。
#ifdef OKD_CALC_EXPORTS
#define OKD_Calc_API __declspec(dllexport)
#else
#define OKD_Calc_API __declspec(dllimport)
#endif

namespace OKD{
namespace Common{

		// このクラスは OKD_Calc.dll からエクスポートされました。
		class OKD_Calc_API CalcByValue {
		public:
			// コンストラクタ
			CalcByValue();

			// 変数・演算式設定情報の初期化
			virtual void Initialize();				// 戻り値なし

			// 変数名と変数値の設定
			virtual int SetVariable(				// 戻り値(0：成功、1：失敗)
				int numVariable,					// 変数の総数
				std::string strVariableName[],		// 変数名の配列
				double valVariableVal[],			// 変数値の配列
				std::string &strErrorMessage);		// エラー情報文字列（エラー内容・問題のある変数名など）※英文とする

			// 演算式名と演算式と演算結果値格納領域の設定
			virtual int SetExpression(				// 戻り値(0：成功、1：失敗)
				int numExpression,					// 演算式の総数
				std::string strCalcName[],			// 演算式名
				std::string strExpression[],		// 演算式の配列
				std::string &strErrorMessage);		// エラー情報文字列（エラー内容・問題のある演算式名・演算式内容など）※英文とする

			// 演算実行
			virtual int Execute(					// 戻り値(0：成功、1：失敗)
				std::string &strErrorMessage);		// エラー情報文字列※英文とする
			
			// 演算結果取得
			virtual int GetCalcResult(				// 戻り値(0：成功、1：失敗)
				std::string strCalcResultName,		// 演算式名
				double &valCalResult);				// 演算結果値

			// Excel用演算式取得
			virtual int GetExcelFormatExpression(	// 戻り値(0：成功、1：失敗)
				std::string strCalcName,			// 演算式名
				std::string &strExpression);		// EXCEL形式に変換した演算式名


		private:
			void InitBuf(void);
			void SetSelectDat(void);
//ADD 2009-11-17
			void SetSelectDat2(void);
//ADD 2009-11-17

			int GetSelectDat(std::string name, std::string & ResultName);

		};

	////////////////////////////////////////////////////////////////////////////////////////////////

		// このクラスは OKD_Calc.dll からエクスポートされました。
		class OKD_Calc_API CalcByMemory
		{
		public:
			// コンストラクタ
			CalcByMemory();

			// デストラクタ
			~CalcByMemory();

			// 定数・変数・演算式設定情報の初期化
			virtual void Initialize();				// 戻り値なし

			// 定数名と定数値の設定
			virtual int SetConstant(				// 戻り値(0：成功、1：失敗)
				int numConstant,					// 定数の総数
				std::string strConstantName[],		// 定数名の配列
				double valConstant[],				// 定数値の配列
				std::string &strErrorMessage);		// エラー情報文字列（エラー内容・問題のある定数名など）※英文とする

			// 変数名と変数値格納領域の設定
			virtual int SetVariable(				// 戻り値(0：成功、1：失敗)
				int numVariable,					// 変数の総数
				std::string strVariableName[],		// 変数名の配列
				double *ptrVariableVal[],			// 変数値の格納領域のポインタの配列
				std::string &strErrorMessage);			// エラー情報文字列（エラー内容・問題のある変数名など）※英文とする

			// 演算式名と演算式と演算結果値格納領域の設定
			virtual int SetExpression(				// 戻り値(0：成功、1：失敗)
				int numExpression,					// 演算式の総数
				std::string strCalcName[],				// 演算式名
				std::string strExpression[],		// 演算式の配列
				double *ptrCalcResultVal[],			// 演算結果値の格納領域のポインタの配列（省略可能）
				std::string &strErrorMessage);		// エラー情報文字列（エラー内容・問題のある演算式名・演算式内容など）※英文とする

			// 演算実行
			virtual int Execute(					// 戻り値(0：成功、1：失敗)
				std::string &strErrorMessage);		// エラー情報文字列※英文とする

			// 演算結果取得
			virtual int GetCalcResult(				// 戻り値(0：成功、1：失敗)
				std::string strCalcResultName,		// 演算式名
				double &valCalResult);				// 演算結果値

			// Excel用演算式取得
			virtual int GetExcelFormatExpression(	// 戻り値(0：成功、1：失敗)
				std::string strCalcName,			// 演算式名
				std::string &strExpression);		// EXCEL形式に変換した演算式名


		private:

			void InitBuf(void);
			void SetNewVariableDat(void);
			void SetSelectDat(void);
//ADD 2009-11-17
			void SetSelectDat2(void);
//ADD 2009-11-17
			void SetCalcResultDat(void);

			int GetSelectDat(std::string name, std::string & ResultName);

		};
}// namespace Common
}// namespace OKD