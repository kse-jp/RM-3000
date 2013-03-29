/////////////////////////////////////////////////////////////////////
//
//  CalcRpn.h
//  ���Z���̉�̓N���X
// 
//	2009-10-08	�쐬	�������r


#ifndef CALCRPN_H
#define CALCRPN_H


#define RPN_REMARK  '#'

struct  rpnErr  {
    string  term ;
    int     code ;
} ;


class	CalcRpn {
public:
	int strRPN(const char *input, char *output, int size ) ;

private:
	char            *p_input ;						// strinput �ւ̃|�C���^
	string          rpn ;
	stack< string > opStack ;
	char            cp_stack;						// *p_stack ���i�[
	int				sign ;

	void	opStackPush( int theOp ) ;
	void	processOp( int theOp ) ;
	int		opAllPop( int theOp ) ;
	int		opPriority( int theOp ) ;
	void	putSpace( void ) ;
	int		putOp( int theOp ) ;
	int		isOperators( int theOp ) ;
	int     preProcess( string &strin, string &strout ) ;
    int     checkItem( string rpn ) ;
	int		checkRPN( string rpn ) ;
    string  errMsg( void ) ;

enum opChar{ opCharNo, opCharStd, opCharLog, opCharFunc, opCharLparen, opCharRparen, opCharComma } ;
} ;

#endif
