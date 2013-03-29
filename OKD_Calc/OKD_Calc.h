/////////////////////////////////////////////////////////////////////
//
//  OKD_Calc.h
//  ���ZDLL�N���X
// 
//	2009-10-08	�쐬	�������r


// �ȉ��� ifdef �u���b�N�� DLL ����̃G�N�X�|�[�g��e�Ղɂ���}�N�����쐬���邽�߂� 
// ��ʓI�ȕ��@�ł��B���� DLL ���̂��ׂẴt�@�C���́A�R�}���h ���C���Œ�`���ꂽ OKD_CALC_EXPORTS
// �V���{���ŃR���p�C������܂��B���̃V���{���́A���� DLL ���g���v���W�F�N�g�Œ�`���邱�Ƃ͂ł��܂���B
// �\�[�X�t�@�C�������̃t�@�C�����܂�ł��鑼�̃v���W�F�N�g�́A 
// OKD_Calc_API �֐��� DLL ����C���|�[�g���ꂽ�ƌ��Ȃ��̂ɑ΂��A���� DLL �́A���̃}�N���Œ�`���ꂽ
// �V���{�����G�N�X�|�[�g���ꂽ�ƌ��Ȃ��܂��B
#ifdef OKD_CALC_EXPORTS
#define OKD_Calc_API __declspec(dllexport)
#else
#define OKD_Calc_API __declspec(dllimport)
#endif

namespace OKD{
namespace Common{

		// ���̃N���X�� OKD_Calc.dll ����G�N�X�|�[�g����܂����B
		class OKD_Calc_API CalcByValue {
		public:
			// �R���X�g���N�^
			CalcByValue();

			// �ϐ��E���Z���ݒ���̏�����
			virtual void Initialize();				// �߂�l�Ȃ�

			// �ϐ����ƕϐ��l�̐ݒ�
			virtual int SetVariable(				// �߂�l(0�F�����A1�F���s)
				int numVariable,					// �ϐ��̑���
				std::string strVariableName[],		// �ϐ����̔z��
				double valVariableVal[],			// �ϐ��l�̔z��
				std::string &strErrorMessage);		// �G���[��񕶎���i�G���[���e�E���̂���ϐ����Ȃǁj���p���Ƃ���

			// ���Z�����Ɖ��Z���Ɖ��Z���ʒl�i�[�̈�̐ݒ�
			virtual int SetExpression(				// �߂�l(0�F�����A1�F���s)
				int numExpression,					// ���Z���̑���
				std::string strCalcName[],			// ���Z����
				std::string strExpression[],		// ���Z���̔z��
				std::string &strErrorMessage);		// �G���[��񕶎���i�G���[���e�E���̂��鉉�Z�����E���Z�����e�Ȃǁj���p���Ƃ���

			// ���Z���s
			virtual int Execute(					// �߂�l(0�F�����A1�F���s)
				std::string &strErrorMessage);		// �G���[��񕶎��񁦉p���Ƃ���
			
			// ���Z���ʎ擾
			virtual int GetCalcResult(				// �߂�l(0�F�����A1�F���s)
				std::string strCalcResultName,		// ���Z����
				double &valCalResult);				// ���Z���ʒl

			// Excel�p���Z���擾
			virtual int GetExcelFormatExpression(	// �߂�l(0�F�����A1�F���s)
				std::string strCalcName,			// ���Z����
				std::string &strExpression);		// EXCEL�`���ɕϊ��������Z����


		private:
			void InitBuf(void);
			void SetSelectDat(void);
//ADD 2009-11-17
			void SetSelectDat2(void);
//ADD 2009-11-17

			int GetSelectDat(std::string name, std::string & ResultName);

		};

	////////////////////////////////////////////////////////////////////////////////////////////////

		// ���̃N���X�� OKD_Calc.dll ����G�N�X�|�[�g����܂����B
		class OKD_Calc_API CalcByMemory
		{
		public:
			// �R���X�g���N�^
			CalcByMemory();

			// �f�X�g���N�^
			~CalcByMemory();

			// �萔�E�ϐ��E���Z���ݒ���̏�����
			virtual void Initialize();				// �߂�l�Ȃ�

			// �萔���ƒ萔�l�̐ݒ�
			virtual int SetConstant(				// �߂�l(0�F�����A1�F���s)
				int numConstant,					// �萔�̑���
				std::string strConstantName[],		// �萔���̔z��
				double valConstant[],				// �萔�l�̔z��
				std::string &strErrorMessage);		// �G���[��񕶎���i�G���[���e�E���̂���萔���Ȃǁj���p���Ƃ���

			// �ϐ����ƕϐ��l�i�[�̈�̐ݒ�
			virtual int SetVariable(				// �߂�l(0�F�����A1�F���s)
				int numVariable,					// �ϐ��̑���
				std::string strVariableName[],		// �ϐ����̔z��
				double *ptrVariableVal[],			// �ϐ��l�̊i�[�̈�̃|�C���^�̔z��
				std::string &strErrorMessage);			// �G���[��񕶎���i�G���[���e�E���̂���ϐ����Ȃǁj���p���Ƃ���

			// ���Z�����Ɖ��Z���Ɖ��Z���ʒl�i�[�̈�̐ݒ�
			virtual int SetExpression(				// �߂�l(0�F�����A1�F���s)
				int numExpression,					// ���Z���̑���
				std::string strCalcName[],				// ���Z����
				std::string strExpression[],		// ���Z���̔z��
				double *ptrCalcResultVal[],			// ���Z���ʒl�̊i�[�̈�̃|�C���^�̔z��i�ȗ��\�j
				std::string &strErrorMessage);		// �G���[��񕶎���i�G���[���e�E���̂��鉉�Z�����E���Z�����e�Ȃǁj���p���Ƃ���

			// ���Z���s
			virtual int Execute(					// �߂�l(0�F�����A1�F���s)
				std::string &strErrorMessage);		// �G���[��񕶎��񁦉p���Ƃ���

			// ���Z���ʎ擾
			virtual int GetCalcResult(				// �߂�l(0�F�����A1�F���s)
				std::string strCalcResultName,		// ���Z����
				double &valCalResult);				// ���Z���ʒl

			// Excel�p���Z���擾
			virtual int GetExcelFormatExpression(	// �߂�l(0�F�����A1�F���s)
				std::string strCalcName,			// ���Z����
				std::string &strExpression);		// EXCEL�`���ɕϊ��������Z����


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