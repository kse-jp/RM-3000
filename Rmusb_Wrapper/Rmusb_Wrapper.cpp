// ����� ���C�� DLL �t�@�C���ł��B

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
	//		�ڑ����u�̔F�����s
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Init(					// �߂�l(0�F�����A1�F���s)
				array<Byte> ^buf,					// ���u�F���o�b�t�@ 
				unsigned char len)					// �F�����悤�Ƃ��鑕�u��(�o�b�t�@�T�C�Y)
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		//int ret = rmusb->Usb_Init(ptr, len);
		int ret = dll_Usb_Init(ptr, len);
		ptr = nullptr;    //unpin:fix����
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		�ڑ����u�̔F�����
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_End(						// �߂�l(0�F�����A1�F���s)
				array<Byte> ^buf,					// ���u�F���o�b�t�@ 
				unsigned char len)					// ������悤�Ƃ��鑕�u��(�o�b�t�@�T�C�Y)
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		//int ret = rmusb->Usb_End(ptr, len);
		int ret = dll_Usb_End(ptr, len);
		ptr = nullptr;    //unpin:fix����
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		�f�o�C�X�ւ̃f�[�^���M
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Data_Write(				// �߂�l(0�F�����A1�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				array<Byte> ^buf,					// ���M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned long len,					// ��]����A���M�f�[�^�T�C�Y
				unsigned long% backlen)				// ���ۂɑ��M�ł����f�[�^�T�C�Y�i�[�|�C���^
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		unsigned long backlen_Res;

		//int ret = rmusb->Usb_Data_Write(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Data_Write(hban, (char*)ptr, len, &backlen_Res);
		ptr = nullptr;    //unpin:fix����
		backlen = backlen_Res;
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		�f�o�C�X����̌v���f�[�^��M
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Data_Read(				// �߂�l(0�F�����A1�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				array<Byte> ^buf,					// ��M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned long len,					// ��]����A��M�f�[�^�T�C�Y
				unsigned long% backlen)			// ���ۂɎ�M�ł����f�[�^�T�C�Y�i�[�|�C���^
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		unsigned long backlen_Res;

		//int ret = rmusb->Usb_Data_Read(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Data_Read(hban, (char*)ptr, len, &backlen_Res);
		ptr = nullptr;    //unpin:fix����
		backlen = backlen_Res;
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		�f�o�C�X�ւ̃R�}���h/�p�����[�^���M
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Cmd_Write(				// �߂�l(0�F�����A1�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				array<Byte> ^buf,					// �R�}���h/�p�����[�^���M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned short len,					// ��]����A���M�f�[�^�T�C�Y
				unsigned short% backlen)				// ���ۂɑ��M�ł����f�[�^�T�C�Y�i�[�|�C���^
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		unsigned short backlen_Res;

		//int ret = rmusb->Usb_Cmd_Write(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Cmd_Write(hban, (char*)ptr, len, &backlen_Res);
		ptr = nullptr;    //unpin:fix����
		backlen = backlen_Res;
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		�f�o�C�X����̃X�e�[�^�X��M
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Status_Read(				// �߂�l(0�F�����A1�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				array<Byte> ^buf,					// ��M�X�e�[�^�X�i�[�o�b�t�@�|�C���^
				unsigned short len,					// ��]����A��M�X�e�[�^�X�T�C�Y
				unsigned short% backlen)				// ���ۂɎ�M�ł����X�e�[�^�X�T�C�Y�i�[�|�C���^
	{

	    pin_ptr<Byte> ptr = &buf[0]; 
		unsigned short backlen_Res;

		//int ret = rmusb->Usb_Status_Read(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Status_Read(hban, (char*)ptr, len, &backlen_Res);
		ptr = nullptr;    //unpin:fix����
		backlen = backlen_Res;
		return ret;
	}

	//----------------------------------------------------------------------------
	// FUNCTION NAME : 
	//
	// INPUT OUTPUT :
	//----------------------------------------------------------------------------
	// FUNCTION OF PROCEDURE
	//		���ݑ��M�҂��ɂȂ��Ă���v���f�[�^�T�C�Y�̎擾
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Get_RemainSize(			// �߂�l(0�F�����A1�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				unsigned long% remainSize)			// ��M�f�[�^�i�[�o�b�t�@�|�C���^
				//						,			
				//unsigned long len,					// ��]����A��M�f�[�^�T�C�Y
				//unsigned long% backlen)				// ���ۂɎ�M�ł����X�e�[�^�X�T�C�Y�i�[�|�C���^
	{


	    //pin_ptr<Byte> ptr = &buf[0]; 
		unsigned long remainSize_Res;
		unsigned short backlen_Res;

		//int ret = rmusb->Usb_Get_Rema9nSize(hban, (char*)ptr, len, &backlen_Res);
		int ret = dll_Usb_Get_RemainSize(hban, (char*)&remainSize_Res, 4, &backlen_Res);
		//ptr = nullptr;    //unpin:fix����
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
	//		���ݑ��M�҂��ɂȂ��Ă���v���f�[�^�T�C�Y�̎擾
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::Usb_Set_Wtime(				// �߂�l(0�F�����A1�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				unsigned long wtime)				// �^�C���A�E�g�l(ms�P��)
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
	//		�F�����u��USB�K�i���Z�b�g���s
	//----------------------------------------------------------------------------
	int Rmusb_Wrapper::UsbResetDevice(				// �߂�l(0�F�����A1�F���s)
				unsigned char hban)					// ���Z�b�g���鑕�u�ԍ�(0�l�`) 
	{
		//int ret = rmusb->UsbResetDevice(hban);
		int ret = dll_UsbResetDevice(hban);
		return ret;
	}

#pragma endregion

} // namespace Common
} // namespace OKD
