// これは メイン DLL ファイルです。

#include "stdafx.h"

#include "msclr/marshal_cppstd.h"

#include "Rmusb_Wrapper.h"
//#pragma comment( lib, "../Output/Rmusb001.lib")

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;
using namespace System::Reflection;


using namespace msclr::interop;


namespace OKD
{
namespace Common
{

#pragma region Rmusb_Wrapper
	
	//----------------------------------------------------------------------------
	// FUNCTION NAME : Constructor
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//----------------------------------------------------------------------------
	Rmusb_Wrapper::Rmusb_Wrapper()
	{
		//rmusb = new OKD::Common::Rmusb();
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : Destructor
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//----------------------------------------------------------------------------
	Rmusb_Wrapper::~Rmusb_Wrapper()
	{
		//delete rmusb;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		接続装置の認識実行
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Init(					// 戻り値(0：成功、1：失敗)
				array<Byte> ^buf,					// 装置認識バッファ 
				unsigned char len)					// 認識しようとする装置数(バッファサイズ)
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		//int ret = rmusb->Usb_Init(ptr, len);
		int ret = dll_Usb_Init(ptr, len);
		ptr = nullptr;    //unpin:fix解除
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		接続装置の認識解放
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_End(						// 戻り値(0：成功、1：失敗)
				array<Byte> ^buf,					// 装置認識バッファ 
				unsigned char len)					// 解放しようとする装置数(バッファサイズ)
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		//int ret = rmusb->Usb_End(ptr, len);
		int ret = dll_Usb_End(ptr, len);
		ptr = nullptr;    //unpin:fix解除
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		デバイスへのデータ送信
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Data_Write(				// 戻り値(0：成功、1：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				array<Byte> ^buf,					// 送信データ格納バッファポインタ
				unsigned long len,					// 希望する、送信データサイズ
				unsigned long% backlen)				// 実際に送信できたデータサイズ格納ポインタ
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		unsigned long backlen_Res;

		//int ret = rmusb->Usb_Data_Write(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Data_Write(hban, (char*)ptr, len, &backlen_Res);
		ptr = nullptr;    //unpin:fix解除
		backlen = backlen_Res;
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		デバイスからの計測データ受信
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Data_Read(				// 戻り値(0：成功、1：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				array<Byte> ^buf,					// 受信データ格納バッファポインタ
				unsigned long len,					// 希望する、受信データサイズ
				unsigned long% backlen)			// 実際に受信できたデータサイズ格納ポインタ
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		unsigned long backlen_Res;

		//int ret = rmusb->Usb_Data_Read(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Data_Read(hban, (char*)ptr, len, &backlen_Res);
		ptr = nullptr;    //unpin:fix解除
		backlen = backlen_Res;
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		デバイスへのコマンド/パラメータ送信
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Cmd_Write(				// 戻り値(0：成功、1：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				array<Byte> ^buf,					// コマンド/パラメータ送信データ格納バッファポインタ
				unsigned short len,					// 希望する、送信データサイズ
				unsigned short% backlen)				// 実際に送信できたデータサイズ格納ポインタ
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		unsigned short backlen_Res;

		//int ret = rmusb->Usb_Cmd_Write(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Cmd_Write(hban, (char*)ptr, len, &backlen_Res);
		ptr = nullptr;    //unpin:fix解除
		backlen = backlen_Res;
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		デバイスからのステータス受信
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Status_Read(				// 戻り値(0：成功、1：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				array<Byte> ^buf,					// 受信ステータス格納バッファポインタ
				unsigned short len,					// 希望する、受信ステータスサイズ
				unsigned short% backlen)				// 実際に受信できたステータスサイズ格納ポインタ
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		unsigned short backlen_Res;

		//int ret = rmusb->Usb_Status_Read(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Status_Read(hban, (char*)ptr, len, &backlen_Res);
		ptr = nullptr;    //unpin:fix解除
		backlen = backlen_Res;
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		現在送信待ちになっている計測データサイズの取得
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Get_RemainSize(			// 戻り値(0：成功、1：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				unsigned long% remainSize)			// 受信データ格納バッファポインタ
				//						,			
				//unsigned long len,					// 希望する、受信データサイズ
				//unsigned long% backlen)				// 実際に受信できたステータスサイズ格納ポインタ
	{


	    //pin_ptr<Byte> ptr = &buf[0]; 
		unsigned long remainSize_Res;
		unsigned short backlen_Res;

		//int ret = rmusb->Usb_Get_Rema9nSize(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Get_RemainSize(hban, (char*)&remainSize_Res, 4, &backlen_Res);
		//ptr = nullptr;    //unpin:fix解除
		remainSize = remainSize_Res;
		//backlen = backlen_Res;
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		現在送信待ちになっている計測データサイズの取得
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Set_Wtime(				// 戻り値(0：成功、1：失敗)
				unsigned char hban,					// 装置番号(0値～) 
				unsigned long wtime)				// タイムアウト値(ms単位)
	{
		//int ret = rmusb->Usb_Set_Wtime(hban, wtime);
		int ret = dll_Usb_Set_Wtime(hban, wtime);
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		認識装置のUSB規格リセット実行
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::UsbResetDevice(				// 戻り値(0：成功、1：失敗)
				unsigned char hban)					// リセットする装置番号(0値～) 
	{
		//int ret = rmusb->UsbResetDevice(hban);
		int ret = dll_UsbResetDevice(hban);
		return ret;
	}

#pragma endregion

} // namespace Common
} // namespace OKD
