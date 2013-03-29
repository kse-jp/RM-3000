/////////////////////////////////////////////////////////////////////
//
//  CalcEngine.h
//  ‰‰Zˆ—ƒNƒ‰ƒX
// 
//	2009-10-08	ì¬	’†‰ª¹r


#ifndef CALCENGINE_H
#define CALCENGINE_H

#include <stack>
#include <map>

class	CalcEngine
{
public:
	int SetVariable(string strVariableName, double valVariableVal);
	int SetRpn(string Symbol, string strRpn); 
	int GetRpn(string &Symbol, string &strRpn); 
    bool        isSymbol( string symbol ) ;
    void        setSymbol( string symbol, long double v ) ;
    long double getSymbol( string symbol ) ;
	void	    rpnOperation( const char *rpn, char *output, int size ) ;
	string	    rpnOperation( void ) ;
	int	        strVariables( string variable) ;
	int	        getVariables( char *variable, int size ) ;
	void		init(void);

private:
	void 		pushVal( string str ) ;
	void 		pushVal( long double val )  ;
	long double	topVal( void ) ;
	void 		topVal( string & str ) ;
	void		popVal( string & str ) ;
	long double popVal( void ) ;

	long double StrTold( char *str ) ;
	long double Logic( char *line, long double vari1, long double vari2 ) ;
	long double Function( long double vari2 ) ;

	struct	operand {
		int		type ;
		string	Str ;
    	long double Val ;
	} ;

	typedef struct operand opStack ;

	char	errstr[1024] ;
	char 	*strRpnOperation;				//*strrpn ;
    map< string, long double > vMap ;
    map< string, string > rpnMap ;
	stack<opStack> Stack ;

} ;

#endif
