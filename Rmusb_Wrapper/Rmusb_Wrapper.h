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

	//pin_ptr��p����C++/CLI�ɂ�郉�b�p 
	public ref class Rmusb_Wrapper {


		[DllImport("rmusb001" ,EntryPoint="Usb_Init") ]
			//�ڑ����u�̔F�����s
			static int dll_Usb_Init(					// �߂�l(0�F�����A1�F���s)
				unsigned char *buf,					// ���u�F���o�b�t�@ 
				unsigned char len);					// �F�����悤�Ƃ��鑕�u��(�o�b�t�@�T�C�Y)

		[DllImport("rmusb001" ,EntryPoint="Usb_End") ]
			//�ڑ����u�̔F�����
			static int dll_Usb_End(					// �߂�l(0�F�����A1�F���s)
				unsigned char *buf,					// ���u�F���o�b�t�@ 
				unsigned char len);					// ������悤�Ƃ��鑕�u��(�o�b�t�@�T�C�Y)

		[DllImport("rmusb001" ,EntryPoint="Usb_Data_Write") ]
			// �f�o�C�X�ւ̃f�[�^���M
			static int dll_Usb_Data_Write(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				char *buf,							// ���M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned long len,					// ��]����A���M�f�[�^�T�C�Y
				unsigned long* backlen);			// ���ۂɑ��M�ł����f�[�^�T�C�Y�i�[�|�C���^

		[DllImport("rmusb001" ,EntryPoint="Usb_Data_Read") ]
			// �f�o�C�X����̌v���f�[�^��M
			static int dll_Usb_Data_Read(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				char *buf,					// ��M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned long len,					// ��]����A��M�f�[�^�T�C�Y
				unsigned long* backlen);			// ���ۂɎ�M�ł����f�[�^�T�C�Y�i�[�|�C���^

		[DllImport("rmusb001" ,EntryPoint="Usb_Cmd_Write") ]
			// �f�o�C�X�ւ̃R�}���h/�p�����[�^���M
			static int dll_Usb_Cmd_Write(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				char *buf,					// �R�}���h/�p�����[�^���M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned short len,					// ��]����A���M�f�[�^�T�C�Y
				unsigned short* backlen);			// ���ۂɑ��M�ł����f�[�^�T�C�Y�i�[�|�C���^

		[DllImport("rmusb001" ,EntryPoint="Usb_Status_Read") ]
			// �f�o�C�X����̃X�e�[�^�X��M
			static int dll_Usb_Status_Read(			// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				char *buf,					// ��M�X�e�[�^�X�i�[�o�b�t�@�|�C���^
				unsigned short len,					// ��]����A��M�X�e�[�^�X�T�C�Y
				unsigned short* backlen);			// ���ۂɎ�M�ł����X�e�[�^�X�T�C�Y�i�[�|�C���^

		[DllImport("rmusb001" ,EntryPoint="Usb_Get_RemainSize") ]
			// ���ݑ��M�҂��ɂȂ��Ă���v���f�[�^�T�C�Y�̎擾
			static int dll_Usb_Get_RemainSize(			// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				char *buf,					// ��M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned short len,					// ��]����A��M�f�[�^�T�C�Y
				unsigned short* backlen);			// ���ۂɎ�M�ł����X�e�[�^�X�T�C�Y�i�[�|�C���^

		[DllImport("rmusb001" ,EntryPoint="Usb_Set_Wtime") ]
			// ���ݑ��M�҂��ɂȂ��Ă���v���f�[�^�T�C�Y�̎擾
			static int dll_Usb_Set_Wtime(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				unsigned long wtime);				// �^�C���A�E�g�l(ms�P��)

		[DllImport("rmusb001" ,EntryPoint="Usb_ResetDevice") ]
			// �F�����u��USB�K�i���Z�b�g���s
			static int dll_UsbResetDevice(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban);				// ���Z�b�g���鑕�u�ԍ�(0�l�`) 



		public:
			// �R���X�g���N�^
			Rmusb_Wrapper();
			// �f�X�g���N�^
			~Rmusb_Wrapper();

			//�ڑ����u�̔F�����s
			virtual int Usb_Init(					// �߂�l(0�F�����A1�F���s)
				array<Byte> ^buf,					// ���u�F���o�b�t�@ 
				unsigned char len);					// �F�����悤�Ƃ��鑕�u��(�o�b�t�@�T�C�Y)

			//�ڑ����u�̔F�����
			virtual int Usb_End(					// �߂�l(0�F�����A1�F���s)
				array<Byte> ^buf,					// ���u�F���o�b�t�@ 
				unsigned char len);					// ������悤�Ƃ��鑕�u��(�o�b�t�@�T�C�Y)

			// �f�o�C�X�ւ̃f�[�^���M
			virtual int Usb_Data_Write(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				array<Byte> ^buf,					// ���M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned long len,					// ��]����A���M�f�[�^�T�C�Y
				unsigned long% backlen);			// ���ۂɑ��M�ł����f�[�^�T�C�Y�i�[�|�C���^

			// �f�o�C�X����̌v���f�[�^��M
			virtual int Usb_Data_Read(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				array<Byte> ^buf,					// ��M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned long len,					// ��]����A��M�f�[�^�T�C�Y
				unsigned long% backlen);			// ���ۂɎ�M�ł����f�[�^�T�C�Y�i�[�|�C���^

			// �f�o�C�X�ւ̃R�}���h/�p�����[�^���M
			virtual int Usb_Cmd_Write(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				array<Byte> ^buf,					// �R�}���h/�p�����[�^���M�f�[�^�i�[�o�b�t�@�|�C���^
				unsigned short len,					// ��]����A���M�f�[�^�T�C�Y
				unsigned short% backlen);			// ���ۂɑ��M�ł����f�[�^�T�C�Y�i�[�|�C���^

			// �f�o�C�X����̃X�e�[�^�X��M
			virtual int Usb_Status_Read(			// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				array<Byte> ^buf,					// ��M�X�e�[�^�X�i�[�o�b�t�@�|�C���^
				unsigned short len,					// ��]����A��M�X�e�[�^�X�T�C�Y
				unsigned short% backlen);			// ���ۂɎ�M�ł����X�e�[�^�X�T�C�Y�i�[�|�C���^

			// ���ݑ��M�҂��ɂȂ��Ă���v���f�[�^�T�C�Y�̎擾
			virtual int Usb_Get_RemainSize(			// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				unsigned long% remainSize);			// ��M�f�[�^�i�[�o�b�t�@�|�C���^
				//						,			
				//unsigned long len,					// ��]����A��M�f�[�^�T�C�Y
				//unsigned long% backlen);			// ���ۂɎ�M�ł����X�e�[�^�X�T�C�Y�i�[�|�C���^

			// ���ݑ��M�҂��ɂȂ��Ă���v���f�[�^�T�C�Y�̎擾
			virtual int Usb_Set_Wtime(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban,					// ���u�ԍ�(0�l�`) 
				unsigned long wtime);				// �^�C���A�E�g�l(ms�P��)

			// �F�����u��USB�K�i���Z�b�g���s
			virtual int UsbResetDevice(				// �߂�l(0�F�����A!0�F���s)
				unsigned char hban);				// ���Z�b�g���鑕�u�ԍ�(0�l�`) 

		private:
			//�V�~�����[�^
			//OKD::Common::Rmusb* rmusb;
	
	};
	#pragma endregion 

	#pragma region Tools

	// std::string����System::String�ւ̕ϊ�
	System::String^ ToCLI(
	  std::string& input,
	  System::Text::Encoding^ encoding)
	{
	  return gcnew System::String(
		input.data(), 0, input.size(), encoding);
	}; 

	// System::String����std::string�ւ̕ϊ�
	std::string FromCLI(
	  String^ input,
	  System::Text::Encoding^ encoding) {
	  std::string result;
	  if ( input != nullptr &&  input->Length > 0 ) {
		array<Byte>^ barray =
		  System::Text::Encoding::Convert(
			  System::Text::Encoding::Unicode, // �ϊ����G���R�[�f�B���O
			  encoding,          // �ϊ���G���R�[�f�B���O
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
