// Rmusb_Wrapper.h

#pragma once

//#include "Rmusb.h"
//#pragma comment( lib, "../Output/Rmusb001.lib")

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;

namespace OKD
{
namespace Common
{

	#pragma region Rmusb_Wrapper

	//pin_ptrを用いたC++/CLIによるラッパ 
	public ref class Rmusb_Wrapper {


		[DllImport("rmusb001" ,EntryPoint="Usb_Init") ]
			//接続装置の認識実行
			static int dll_Usb_Init(					// 戻り値(0：成功、1：失敗)
				unsigned char *buf,					// 装置認識バッファ 
				unsigned char len);					// 認識しようとする装置数(バッファサイズ)

		[DllImport("rmusb001" ,EntryPoint="Usb_End") ]
			//接続装置の認識解放
			static int dll_Usb_End(					// 戻り値(0：成功、1：失敗)
				unsigned char *buf,					// 装置認識バッファ 
				unsigned char len);					// 解放しようとする装置数(バッファサイズ)

		[DllImport("rmusb001" ,EntryPoint="Usb_Data_Write") ]
			// デバイスへのデータ送信
			static int dll_Usb_Data_Write(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				char *buf,							// 送信データ格納バッファポインタ
				unsigned long len,					// 希望する、送信データサイズ
				unsigned long* backlen);			// 実際に送信できたデータサイズ格納ポインタ

		[DllImport("rmusb001" ,EntryPoint="Usb_Data_Read") ]
			// デバイスからの計測データ受信
			static int dll_Usb_Data_Read(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				char *buf,					// 受信データ格納バッファポインタ
				unsigned long len,					// 希望する、受信データサイズ
				unsigned long* backlen);			// 実際に受信できたデータサイズ格納ポインタ

		[DllImport("rmusb001" ,EntryPoint="Usb_Cmd_Write") ]
			// デバイスへのコマンド/パラメータ送信
			static int dll_Usb_Cmd_Write(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				char *buf,					// コマンド/パラメータ送信データ格納バッファポインタ
				unsigned short len,					// 希望する、送信データサイズ
				unsigned short* backlen);			// 実際に送信できたデータサイズ格納ポインタ

		[DllImport("rmusb001" ,EntryPoint="Usb_Status_Read") ]
			// デバイスからのステータス受信
			static int dll_Usb_Status_Read(			// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				char *buf,					// 受信ステータス格納バッファポインタ
				unsigned short len,					// 希望する、受信ステータスサイズ
				unsigned short* backlen);			// 実際に受信できたステータスサイズ格納ポインタ

		[DllImport("rmusb001" ,EntryPoint="Usb_Get_RemainSize") ]
			// 現在送信待ちになっている計測データサイズの取得
			static int dll_Usb_Get_RemainSize(			// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				char *buf,					// 受信データ格納バッファポインタ
				unsigned short len,					// 希望する、受信データサイズ
				unsigned short* backlen);			// 実際に受信できたステータスサイズ格納ポインタ

		[DllImport("rmusb001" ,EntryPoint="Usb_Set_Wtime") ]
			// 現在送信待ちになっている計測データサイズの取得
			static int dll_Usb_Set_Wtime(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				unsigned long wtime);				// タイムアウト値(ms単位)

		[DllImport("rmusb001" ,EntryPoint="Usb_ResetDevice") ]
			// 認識装置のUSB規格リセット実行
			static int dll_UsbResetDevice(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban);				// リセットする装置番号(0値～) 



		public:
			// コンストラクタ
			Rmusb_Wrapper();
			// デストラクタ
			~Rmusb_Wrapper();

			//接続装置の認識実行
			virtual int Usb_Init(					// 戻り値(0：成功、1：失敗)
				array<Byte> ^buf,					// 装置認識バッファ 
				unsigned char len);					// 認識しようとする装置数(バッファサイズ)

			//接続装置の認識解放
			virtual int Usb_End(					// 戻り値(0：成功、1：失敗)
				array<Byte> ^buf,					// 装置認識バッファ 
				unsigned char len);					// 解放しようとする装置数(バッファサイズ)

			// デバイスへのデータ送信
			virtual int Usb_Data_Write(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				array<Byte> ^buf,					// 送信データ格納バッファポインタ
				unsigned long len,					// 希望する、送信データサイズ
				unsigned long% backlen);			// 実際に送信できたデータサイズ格納ポインタ

			// デバイスからの計測データ受信
			virtual int Usb_Data_Read(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				array<Byte> ^buf,					// 受信データ格納バッファポインタ
				unsigned long len,					// 希望する、受信データサイズ
				unsigned long% backlen);			// 実際に受信できたデータサイズ格納ポインタ

			// デバイスへのコマンド/パラメータ送信
			virtual int Usb_Cmd_Write(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				array<Byte> ^buf,					// コマンド/パラメータ送信データ格納バッファポインタ
				unsigned short len,					// 希望する、送信データサイズ
				unsigned short% backlen);			// 実際に送信できたデータサイズ格納ポインタ

			// デバイスからのステータス受信
			virtual int Usb_Status_Read(			// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				array<Byte> ^buf,					// 受信ステータス格納バッファポインタ
				unsigned short len,					// 希望する、受信ステータスサイズ
				unsigned short% backlen);			// 実際に受信できたステータスサイズ格納ポインタ

			// 現在送信待ちになっている計測データサイズの取得
			virtual int Usb_Get_RemainSize(			// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				unsigned long% remainSize);			// 受信データ格納バッファポインタ
				//						,			
				//unsigned long len,					// 希望する、受信データサイズ
				//unsigned long% backlen);			// 実際に受信できたステータスサイズ格納ポインタ

			// 現在送信待ちになっている計測データサイズの取得
			virtual int Usb_Set_Wtime(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				unsigned long wtime);				// タイムアウト値(ms単位)

			// 認識装置のUSB規格リセット実行
			virtual int UsbResetDevice(				// 戻り値(0：成功、!0：失敗)
				unsigned char hban);				// リセットする装置番号(0値～) 

		private:
			//シミュレータ
			//OKD::Common::Rmusb* rmusb;
	
	};
	#pragma endregion 

	#pragma region Tools

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
	#pragma endregion 


} // namespace Common
} // namespace OKD
