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
			// �R���X�g���N�^
			CalcByValue_Wrapper();
			// �f�X�g���N�^
			~CalcByValue_Wrapper();

			// �ϐ��E���Z���ݒ���̏�����
			virtual void Initialize();				// �߂�l�Ȃ�

			// �ϐ����ƕϐ��l�̐ݒ�
			virtual int SetVariable(				// �߂�l(0�F�����A1�F���s)
				int numVariable,					// �ϐ��̑���
				array<String^>^ strVariableName,	// �ϐ����̔z��
				array<double>^ valVariableVal,		// �ϐ��l�̔z��
				String^% strErrorMessage);			// �G���[��񕶎���i�G���[���e�E���̂���ϐ����Ȃǁj���p���Ƃ���

			// ���Z�����Ɖ��Z���Ɖ��Z���ʒl�i�[�̈�̐ݒ�
			virtual int SetExpression(				// �߂�l(0�F�����A1�F���s)
				int numExpression,					// ���Z���̑���
				array<String^>^ strCalcName,		// ���Z����
				array<String^>^ strExpression,		// ���Z���̔z��
				String^% strErrorMessage);			// �G���[��񕶎���i�G���[���e�E���̂��鉉�Z�����E���Z�����e�Ȃǁj���p���Ƃ���

			// ���Z���s
			virtual int Execute(					// �߂�l(0�F�����A1�F���s)
				String^% strErrorMessage);			// �G���[��񕶎��񁦉p���Ƃ���
			
			// ���Z���ʎ擾
			virtual int GetCalcResult(				// �߂�l(0�F�����A1�F���s)
				String^ strCalcResultName,			// ���Z����
				double% valCalResult);				// ���Z���ʒl

			// Excel�p���Z���擾
			virtual int GetExcelFormatExpression(	// �߂�l(0�F�����A1�F���s)
				String^ strCalcName,				// ���Z����
				String^% strExpression);			// EXCEL�`���ɕϊ��������Z����

		private:
			OKD::Common::CalcByValue* calc;
	
	};
	#pragma endregion 

	#pragma region CalcByMemory_Wrapper


	////////////////////////////////////////////////////////////////////////////////////////////////

	public ref class CalcByMemory_Wrapper
	{
		public:
			// �R���X�g���N�^
			CalcByMemory_Wrapper();

			// �f�X�g���N�^
			~CalcByMemory_Wrapper();

			// �萔�E�ϐ��E���Z���ݒ���̏�����
			virtual void Initialize();				// �߂�l�Ȃ�

			// �萔���ƒ萔�l�̐ݒ�
			virtual int SetConstant(				// �߂�l(0�F�����A1�F���s)
				int numConstant,					// �萔�̑���
				array<String^>^ strConstantName,	// �萔���̔z��
				array<double>^ valConstant,			// �萔�l�̔z��
				String^% strErrorMessage);			// �G���[��񕶎���i�G���[���e�E���̂���萔���Ȃǁj���p���Ƃ���

			// �ϐ����ƕϐ��l�i�[�̈�̐ݒ�
			virtual int SetVariable(				// �߂�l(0�F�����A1�F���s)
				int numVariable,					// �ϐ��̑���
				array<String^>^ strVariableName,	// �ϐ����̔z��
				array<double> ^ptrVariableVal,		// �ϐ��l�̊i�[�̈�̃|�C���^�̔z��
			    String^% strErrorMessage);			// �G���[��񕶎���i�G���[���e�E���̂���ϐ����Ȃǁj���p���Ƃ���

			// ���Z�����Ɖ��Z���Ɖ��Z���ʒl�i�[�̈�̐ݒ�
			virtual int SetExpression(				// �߂�l(0�F�����A1�F���s)
				int numExpression,					// ���Z���̑���
				array<String^>^ strCalcName,		// ���Z����
				array<String^>^ strExpression,		// ���Z���̔z��
				array<double> ^ptrCalcResultVal,	// �ϐ��l�̊i�[�̈�̃|�C���^�̔z��
				String^% strErrorMessage);		    // �G���[��񕶎���i�G���[���e�E���̂��鉉�Z�����E���Z�����e�Ȃǁj���p���Ƃ���

			// ���Z���s
			virtual int Execute(					// �߂�l(0�F�����A1�F���s)
				String^% strErrorMessage);			// �G���[��񕶎��񁦉p���Ƃ���

			// ���Z���ʎ擾
			virtual int GetCalcResult(				// �߂�l(0�F�����A1�F���s)
				String^ strCalcResultName,			// ���Z����
				double% valCalResult);				// ���Z���ʒl

			// Excel�p���Z���擾
			virtual int GetExcelFormatExpression(	// �߂�l(0�F�����A1�F���s)
				String^ strCalcName,				// ���Z����
				String^% strExpression);			// EXCEL�`���ɕϊ��������Z����

		private:
			OKD::Common::CalcByMemory* calc;

	};
	#pragma endregion



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


} // namespace Common

} // namespace OKD