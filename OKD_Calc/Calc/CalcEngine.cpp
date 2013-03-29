/////////////////////////////////////////////////////////////////////
//
//  CalcEngine.cpp
//  ‰‰Zˆ—ƒNƒ‰ƒX
// 
//	2009-10-08	ì¬	’†‰ª¹r


#include "stdafx.h"

#pragma warning(disable:4786)

////////////////////////////////////////////////////////////////////////////
#pragma warning( disable : 4996 )
#define _CRT_SECURE_NO_DEPRECATE

#ifdef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#undef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1
//strcopy‚âsprintf()‚È‚Ç‚Ì(ƒoƒbƒtƒ@ƒI[ƒoƒ‰ƒ“‘Îô)ƒ[ƒjƒ“ƒO‚ğ—}~‚·‚é
#endif
////////////////////////////////////////////////////////////////////////////



#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <string>

#include <cfloat>


#include <stack>
#include <map>
#include <excpt.h>

using namespace std;

#include "Tool.h"
#include "CalcRpn.h"
#include "CalcEngine.h"
#include "CalcMathErr.h"


extern char ErrorString[];
extern int	calcErrZero;	//DBL_MAX;

//----------------------------------------------------------------------------
// FUNCTION NAME : pushVal
//
// INPUT OUTPUT :
//		string		str			(I)	•¶šî•ñ‚Ì•¶š—ñ
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		•¶šî•ñ‚ğŠm”F‚µA’l‚©×ÍŞÙ•¶š‚ğ“o˜^‚·‚é
//----------------------------------------------------------------------------
void CalcEngine::pushVal( string str )
{
	long double	val ;
    char	*s ;
    opStack	opr ;

	val = strtod( str.c_str(), &s ) ;	//•¶š—ñ‚ğdoubleŒ^‚Ì’l‚É‚·‚é
    if( *s ) {
    	opr.type = 0 ;
        opr.Str = str ;		//×ÍŞÙ‚Æ‚µ‚Ä“o˜^
        opr.Val = 0 ;
    } else {
    	opr.type = 1 ;
        opr.Str = "" ;
        opr.Val = val ;		//’l‚Æ‚µ‚Ä“o˜^
    }
    Stack.push( opr ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : pushVal
//
// INPUT OUTPUT :
//		long double		val		(I)	’l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		’l‚Æ‚µ‚Ä“o˜^‚·‚é
//----------------------------------------------------------------------------
void CalcEngine::pushVal( long double val )
{
    opStack	opr ;

   	opr.type = 1 ;
	opr.Str = "" ;
    opr.Val = val ;
    Stack.push( opr ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : topVal
//
// INPUT OUTPUT :
//		long double		(RETURN)		(R)	“o˜^‚³‚ê‚½’l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		½À¯¸‚Æ‚µ‚Ä•Û‚³‚ê‚½î•ñ(×ÍŞÙ‚Æ’l)‚©‚ç’l‚Æ‚µ‚Äæ“¾‚·‚é
//----------------------------------------------------------------------------
//		’) 0Š„‚Ìî•ñ‚È‚ç0’l‚ğ•Ô‚·		
//----------------------------------------------------------------------------
long double	CalcEngine::topVal( void )
{
    opStack	opr ;
    map< string, long double >::iterator mi ;
    long double val=0.0;

	if( Stack.empty() )
		throw( RunError( "Syntax" ) ) ;
    opr = Stack.top() ;

	if( opr.type == 0 ) {
		// •Ï”(×ÍŞÙ)’è‹`‚³‚ê‚Ä‚¢‚é
		mi = vMap.find( opr.Str ) ;
    	if( mi != vMap.end() ) {
        	val = (*mi).second ;
        } else {
			sprintf( errstr, "Undefined[%s]", opr.Str.c_str() ) ;

			throw RunError( errstr ) ;
        }
	} else {
		//’l
	    val = opr.Val ;
    }

	if( val == DBL_MAX ){
		//0Š„‚Ìî•ñ‚È‚ç
		val = 0.0;
		calcErrZero = 1;
	}
    return( val ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : topVal
//
// INPUT OUTPUT :
//		string &		str		(O)	“o˜^‚³‚ê‚½×ÍŞÙ•¶š—ñ
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		½À¯¸‚Æ‚µ‚Ä•Û‚³‚ê‚½î•ñ‚©‚ç×ÍŞÙ‚ğæ“¾‚·‚é
//----------------------------------------------------------------------------
void CalcEngine::topVal( string & str )
{
    opStack	opr ;

	if( Stack.empty() )
		throw( RunError( "Syntax" ) ) ;
    opr = Stack.top() ;
    str = opr.Str ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : topVal
//
// INPUT OUTPUT :
//		long double		(RETURN)		(R)	“o˜^‚³‚ê‚½’l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		½À¯¸‚Æ‚µ‚Ä•Û‚³‚ê‚½î•ñ(×ÍŞÙ‚Æ’l)‚©‚ç’l‚Æ‚µ‚Äæ“¾‚µ½À¯¸‚ÉÏ‚Ş
//----------------------------------------------------------------------------
//		’) 0Š„‚Ìî•ñ‚È‚ç0’l‚ğ•Ô‚·		
//----------------------------------------------------------------------------
long double CalcEngine::popVal( void )
{
	long double val ;

    val = topVal() ;
    Stack.pop() ;
    return( val ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : popVal
//
// INPUT OUTPUT :
//		string &		str		(O)	“o˜^‚³‚ê‚½×ÍŞÙ•¶š—ñ
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		½À¯¸‚Æ‚µ‚Ä•Û‚³‚ê‚½î•ñ‚©‚ç×ÍŞÙ‚ğæ“¾‚·‚é
//----------------------------------------------------------------------------
void CalcEngine::popVal( string & str )
{
    topVal( str ) ;
    Stack.pop() ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : strVariables
//
// INPUT OUTPUT :
//		string		variable			(I)	“o˜^‚·‚é•Ï”–¼
//		int			(RETURN)			(R)	“o˜^‚µ‚½ŒÂ”
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		•Ï”–¼‚Æ•Ï”’l‚Ìİ’è
//		"\r\n"‚Å‹æØ‚ç‚ê‚½•¡”‚Ìİ’è‚ª‰Â”\
//----------------------------------------------------------------------------
int	CalcEngine::strVariables( string variable)
{
	unsigned int		i ;
    string	name ;
    string	val ;
    string	tmp ;
    long double	v ;
    map< string, long double >::iterator mi ;

	unsigned int k = stringToken(variable, "\r\n");
	std::vector< string > ss( k ) ;
	for(unsigned int ii = 0; ii< k; ii++){
		ss.at(ii) = strTokenBuf[ii];
	}
	for( i = 0; i< k; i++) {
		tmp = ss.at(i);

		unsigned int j = stringToken(tmp, "=");
		for(unsigned int ii = 0; ii< j; ii++){
			ss.push_back(strTokenBuf[ii]);
		}
		name = ss.at(0);
		if( j == 0 ){
			strcpy(ErrorString, name.c_str());
			return -1*(i+1);
		}
		val = ss.at(1);

        v = strtod( val.c_str(), NULL ) ;
        mi = vMap.find( name ) ;
        if( mi == vMap.end() || vMap.size() == 0 ) {
        	vMap.insert( make_pair( name, v ) ) ;
        } else {
        	(*mi).second = v ;
        }
    }

    return( i ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : getVariables
//
// INPUT OUTPUT :
//		char *		variable			(O)	“o˜^‚³‚ê‚Ä‚¢‚é•Ï”–¼
//		int			size				(I)	¾¯Ä‚·‚éÊŞ¯Ì§»²½Ş
//		int			(RETURN)			(R)	“o˜^‚µ‚½ŒÂ”
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		“o˜^‚³‚ê‚Ä‚¢‚é•Ï”–¼‚Ìæ“¾
//----------------------------------------------------------------------------
int	CalcEngine::getVariables( char *variable, int size )
{
//	int		i = 0 ;
	int		i ;
    string	var = "" ;
    char	str[1024] ;
    map< string, long double >::iterator mi ;
    int		n ;

    n = vMap.size() ;
    for( i = 0, mi = vMap.begin() ; i < n ; i++ ) {
    	sprintf( str, "%Lf", (*mi).second ) ;
    	var = var + (*mi).first + '=' + str + "\r\n" ;
        mi++ ;
    }
	strncpy( variable, var.c_str(), size-1 ) ;
    return( i ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : StrTold
//
// INPUT OUTPUT :
//		char *		variable			(I)	•ÏŠ·‚·‚é•¶š—ñ
//		long double	(RETURN)			(R)	æ“¾’l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		”š•¶š—ñ‚©‚ç’l‚É•ÏŠ·
//----------------------------------------------------------------------------
long double CalcEngine::StrTold( char *str )
{
	long	double	vari ;
	char	*s ;

    vari = strtod( str, &s ) ;
	if( *s != '\0' && *s != ' ' ) {
    	sprintf( errstr, "NemericSyntax[%s]", str ) ;
//		throw( RunError( errstr ) ) ;
		throw RunError( errstr ) ;
    }
    return( vari ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : Logic
//
// INPUT OUTPUT :
//		char *		line				(I)	˜_—®(”äŠr‰‰Zq)‚Ì•¶š—ñ
//		long double	vari1				(I)	”äŠr‚·‚é’l1
//		long double	vari2				(I)	”äŠr‚·‚é’l2
//		long double	(RETURN)			(R)	˜_—®(”äŠr)‚ÌŒ‹‰Ê(1/0)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		˜_—®(”äŠr‰‰Zq)‚Ìˆ—
//----------------------------------------------------------------------------
long double CalcEngine::Logic( char *line, long double vari1, long double vari2 )
{
	long double vari0 ;

	if(strcmp( line, ".GT") == 0) {
		vari0 = ( vari1 > vari2 )? 1 : 0 ;
	} else if(strcmp( line, ".GE") == 0) {
		vari0 = ( vari1 >= vari2 )? 1 : 0 ;
	} else if(strcmp( line, ".LT") == 0) {
		vari0 = ( vari1 < vari2 )? 1 : 0 ;
	} else if(strcmp( line, ".LE") == 0) {
		vari0 = ( vari1 <= vari2 )? 1 : 0 ;
	} else if(strcmp( line, ".EQ") == 0) {
		vari0 = ( vari1 == vari2 )? 1 : 0 ;
	} else if(strcmp( line, ".NE") == 0) {
		vari0 = ( vari1 != vari2 )? 1 : 0 ;
	} else {
    	sprintf( errstr, "Undefined Operand[%s]", line ) ;
//		throw( RunError( errstr ) ) ;
		throw RunError( errstr ) ;
    }
    return( vari0 ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : Function
//
// INPUT OUTPUT :
//		long double	vari2				(I)	‰‰Z‚·‚é’l
//		long double	(RETURN)			(R)	‰‰ZŒ‹‰Ê
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		ŠÖ”‚Ìˆ—
//----------------------------------------------------------------------------
long double CalcEngine::Function( long double vari2 )
//static long double Function( long double vari2 )
{
	string		str ;
    long double vari ;
	long double vari1 ;
    long double vari0 ;
	long double varim ;
    char		*strvari1 ;

	topVal( str ) ;
    strvari1 = (char *)str.c_str() ;
	if(strcmp(strvari1, "PI") == 0) {
    	vari = 3.14159265358979 ; //PI() ;
	} else if(strcmp(strvari1, "ABS") == 0) {
   		try {
	    	vari = fabsl( vari2 ) ;
        } catch(...)  {
	    	sprintf( errstr, "%s(%Lf)", strvari1, vari2 ) ;
			throw RunError( errstr ) ;
		}
	} else if(strcmp(strvari1, "FALSE") == 0) {
    	vari = 0 ;
	} else if(strcmp(strvari1, "TRUE") == 0) {
    	vari = 1 ;
	} else if(strcmp(strvari1, "NOT") == 0) {
    	vari = ( vari2 == 0)? 1 : 0 ;
	} else if(strcmp(strvari1, "SQRT") == 0) {
		try {
  			vari =  sqrtl( vari2 ) ;
        }
		catch(...) {
	    	sprintf( errstr, "%s(%Lf)", strvari1, vari2 ) ;
			throw RunError( errstr ) ;
        }

	} else if(strcmp(strvari1, "LN") == 0) {
		try {
  			vari =  logl( vari2 ) ;
        }
		catch(...) {
	    	sprintf( errstr, "%s(%Lf)", strvari1, vari2 ) ;
			throw RunError( errstr ) ;
        }
	} else if(strcmp(strvari1, "LOG") == 0) {
		try {
  			vari =  logl( vari2 ) ;
        }
		catch(...) {
	    	sprintf( errstr, "%s(%Lf)", strvari1, vari2 ) ;
			throw RunError( errstr ) ;
        }
	} else if(strcmp(strvari1, "LOG10") == 0) {
		try {
  			vari =  log10l( vari2 ) ;
        }
		catch(...) {
	    	sprintf( errstr, "%s(%Lf)", strvari1, vari2 ) ;
			throw RunError( errstr ) ;
        }
	} else if(strcmp(strvari1, "EXP") == 0) {
		try {
  			vari =  expl( vari2 ) ;
        }
		catch(...) {
	    	sprintf( errstr, "%s(%Lf)", strvari1, vari2 ) ;
			throw RunError( errstr ) ;
        }
	} else {
		//	‚Q€‰‰Zq
    	vari1 = popVal() ;
		topVal( str ) ;
    	strvari1 = (char *)str.c_str() ;
		if( strcmp( strvari1, "POWER" ) == 0 ) {
			try {
				varim = vari2 ;
				vari = 1 ;
				for( ; ; ) {
					if( varim < 300 ) {
						vari0 = powl(vari1, varim ) ;
						vari *= vari0 ;
						break ;
					}
					varim -= 300 ;
					vari *= powl( vari1, 300 ) ;

				}
        	}
			catch(...) {
	    		sprintf( errstr, "%s(%Lf,%Lf)", strvari1, vari1, vari2 ) ;
				throw RunError( errstr ) ;
        	}
		} else if( strcmp( strvari1, "AND" ) == 0 ) {
        	vari = vari1 * vari2 ;
            if( vari != 0 )
            	vari = 1 ;
		} else if( strcmp( strvari1, "OR" ) == 0 ) {
        	vari = vari1 + vari2 ;
            if( vari != 0 )
            	vari = 1 ;
    	} else {
			//	‚R€‰‰Zq
        	vari0 = popVal() ;
			topVal( str ) ;
		    strvari1 = (char *)str.c_str() ;

			if( strcmp( strvari1, "IF" ) == 0 ) {
               	vari = ( vari0 != 0 )? vari1 : vari2 ;
			} else if( strcmp( strvari1, "AND" ) == 0 ) {
	        	vari = vari1 * vari2 * vari0 ;
	            if( vari != 0 )
	            	vari = 1 ;
			} else if( strcmp( strvari1, "OR" ) == 0 ) {
	        	vari = vari1 + vari2 + vari0 ;
	            if( vari != 0 )
	            	vari = 1 ;
	   		} else {
	    		sprintf( errstr, "UnknownFunction/%s()", strvari1 ) ;
				throw RunError( errstr ) ;
    		}
		}
	}
	popVal( str ) ;
    strvari1 = (char *)str.c_str() ;
    return( vari ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : Arithmetic
//
// INPUT OUTPUT :
//		int			theOp				(I)	ZpŒvZ‹L†
//		long double	vari1				(I)	•Ï”’l
//		long double	vari2				(I)	•Ï”’l
//		long double		(RETURN)		(R)	‰‰ZŒvZ’l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		ZpŒvZ‚Ìˆ—
//----------------------------------------------------------------------------
static long double Arithmetic( int theOp, long double vari1, long double vari2 )
{
	long double vari ;
    char		errstr[1024] ;

	switch( theOp ) {
						case '+':
        	            	vari = vari1 + vari2 ;
							break;
                    	case '-':
                    		vari = vari1 - vari2 ;
							break;
                        case '*':
            				try {
  		        				vari =  vari1 * vari2 ;
                			}
							catch(...) {
	               				sprintf( errstr, "%Lf*%Lf", vari1, vari2 ) ;
								throw RunError( errstr ) ;
                			}
							break;

						case '^':
            				try {
                        		vari =  powl( vari1, vari2 ) ;
                			}
							catch(...) {
	               				sprintf( errstr, "powl(%Lf,%Lf)", vari1, vari2 ) ;
								throw RunError( errstr ) ;
                			}
							break;

						case '/':
                        	if( vari2 == 0 ) {
                            	vari = 0 ;
								calcErrZero = 1;	//0Š„
                            } else {

            					try {
  		        					vari =  vari1 / vari2 ;
                				}
								catch(...) {
	               					sprintf( errstr, "%Lf/%Lf", vari1, vari2 ) ;
									throw RunError( errstr ) ;
                				}
                            }
							break;

						case '%':
            				try {
  		        				vari =  fmodl( vari1,vari2 ) ;
                			}
							catch(...) {
	               				sprintf( errstr, "%Lf/%Lf", vari1, vari2 ) ;
								throw RunError( errstr ) ;
                			}
	}
	return( vari ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetVariable
//
// INPUT OUTPUT :
//		string		strVariableName		(I)	“o˜^‚·‚é•Ï”–¼
//		double		valVariableVal		(I)	“o˜^‚·‚é•Ï”’l
//		int			(RETURN)			(R)	Map“o˜^‚³‚ê‚½ŒÂ”
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		•Ï”–¼‚Æ•Ï”’l‚Ìİ’è
//----------------------------------------------------------------------------
//		’) “o˜^•Ï”–¼‚Í‘å•¶š‚É•ÏŠ·‚µ‚Ä‚ ‚é
//----------------------------------------------------------------------------
int CalcEngine::SetVariable(string strVariableName, double valVariableVal)
{
	int	i = -1;
	map< string, long double >::iterator mi ;
	string	name;
	long double	v = valVariableVal;

	//‘å•¶š‚É•ÏŠ·
	Tool_ChgUpper(strVariableName, name); 

	mi = vMap.find( name ) ;
	if( mi == vMap.end() || vMap.size() == 0 ) {
		//V‹K‚É“o˜^
		vMap.insert( make_pair( name, v ) ) ;   
	} else {
		//Šù‚É“o˜^‚³‚ê‚Ä‚¢‚é‚ªA’l‚Ì‚İ‚ğ•ÏX‚·‚é
		(*mi).second = v ;
	}
	
	i = vMap.size() ; 
    return( i ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : isSymbol
//
// INPUT OUTPUT :
//		string		symbol				(I)	•Ï”–¼
//		int			(RETURN)			(R)	“o˜^Ï‚İÁª¯¸’l(trueF“o˜^Ï‚İAfalseF“o˜^‚³‚ê‚Ä‚È‚¢)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		•Ï”‚Ì“o˜^Ï‚İƒ`ƒFƒbƒN
//----------------------------------------------------------------------------
bool	CalcEngine::isSymbol( string symbol )
{
    map< string, long double >::iterator mi ;

        mi = vMap.find( symbol ) ;
        if( mi == vMap.end() || vMap.size() == 0 ) {
        	return false ;
        } else {
        	return true ;
        }
}

//----------------------------------------------------------------------------
// FUNCTION NAME : setSymbol
//
// INPUT OUTPUT :
//		string			symbol		(I)	•Ï”–¼
//		long double		val			(I)	“o˜^‚·‚é’l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		•Ï”‚Ì“o˜^^•ÏX
//		V‹K‚Ìê‡‚Í“o˜^A“o˜^Ï‚İ‚Ìê‡‚Í’l‚ÌXV‚ğs‚¤
//----------------------------------------------------------------------------
void    CalcEngine::setSymbol( string symbol, long double v )
{
    map< string, long double >::iterator mi ;

        mi = vMap.find( symbol ) ;
        if( mi == vMap.end() || vMap.size() == 0 ) {
        	vMap.insert( make_pair( symbol, v ) ) ;
        } else {
        	(*mi).second = v ;
        }
}

//----------------------------------------------------------------------------
// FUNCTION NAME : getSymbol
//
// INPUT OUTPUT :
//		string			symbol		(I)	•Ï”–¼
//		long double		(RETURN)	(R)	‰‰ZŒvZ’l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		•Ï”‚Ìæ“¾
//----------------------------------------------------------------------------
long double	CalcEngine::getSymbol( string symbol )
{
	string name("");
	long double fVal;

	//‘å•¶š‚É•ÏŠ·
	Tool_ChgUpper(symbol, name); 

    map< string, long double >::iterator mi ;

        mi = vMap.find( name ) ;
        if( mi == vMap.end() || vMap.size() == 0 ) {
        	return 0 ;
        } else {
			fVal = (*mi).second ;
			if( fVal == DBL_MAX ){
				//0Š„‚è‚µ‚½Œ‹‰Ê
				fVal = 0.0;
			}
        	return fVal;
        }
}

//----------------------------------------------------------------------------
// FUNCTION NAME : rpnOperation
//
// INPUT OUTPUT :
//		string		(RETURN)			(R)	‰‰ZŒ‹‰Ê(‰‰Z•Ï”–¼=‰‰Z’l)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		‰‰Zˆ—
//		strRpnOperation‚É¾¯Ä‚³‚ê‚½‰‰Z®‚ğ•]‰¿‚µ‚Ä‰‰Z‚ğÀs‚·‚é
//----------------------------------------------------------------------------
string	CalcEngine::rpnOperation( void )
{
	char		c;
	char		*strvari1 ;
	long double	vari0, vari1, vari2;
    string		str ;
    long double vari ;
    string		Str = "" ;
    string		Rpn( strRpnOperation );		//strrpn ) ;
    unsigned int			term ;
    string		token ;
    char		errmsg[1024] ;

	calcErrZero = 0;	//DBL_MAX;

	for( ; Str.size() > 0 ; )
		Str.erase() ;

	// ŒvZ
 //   //tokenizer<string> var( Rpn, " " ) ;
	unsigned int k = stringToken(Rpn, " ");
	std::vector< string > ss( k ) ;
	for(unsigned int ii = 0; ii< k; ii++){
		ss.at(ii) = strTokenBuf[ii];
	}
    for( ; !Stack.empty() ; ) {
    	Stack.pop() ;
    }

double tTime = 0;
//double tTime = 0;
//Timer_Start();

	try {
		for( term = 0; term< k; term++ ) {
			token = ss.at(term);

			// ‰‰Zq‚©‚Ç‚¤‚©‚Ì”»’è
			c = token[0] ;
			if( (c != '+'  && c != '-' && c != '.' && c != '*' && c != '/' && c != '%' && c != '^' && c != '=' && c != '@')) {
				// ’l‚È‚ç‚ÎƒXƒ^ƒbƒN‚Épush
//Timer_Start();
                pushVal( token ) ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("pushVal()‚Å=%f\n", tTime);
//#endif
			} else {
				//‰‰Zq
				if(c == '@') {
//Timer_Start();
	                vari2 = popVal() ;
					vari0 = Function( vari2 ) ;
                    pushVal( vari0 ) ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("@ popVal/pushVal()‚Å=%f\n", tTime);
//#endif
				} else if(c != '=') {
	                vari2 = popVal() ;
                	vari1 = popVal() ;

					switch (c) {
						case '+':
                    	case '-':
                        case '*':
						case '^':
						case '/':
						case '%':
							if( calcErrZero ){
								//0Š„‚ª”­¶‚µ‚½ÃŞ°À‚ğg—p‚µ‚½
	                        	pushVal( 0.0 ) ;
							}else{
//double tTime = 0;
//Timer_Start();
								vari = Arithmetic( c, vari1, vari2 ) ;
	                        	pushVal( vari ) ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("Arithmetic()‚Å=%f\n", tTime);
//#endif
							}
							break;
						case '.':
        					vari0 = Logic( (char *)token.c_str(), vari1, vari2 ) ;
        	                pushVal( vari0 ) ;
							break;
					}
				} else {		//'='
//Timer_Start();
	                vari2 = popVal() ;
					popVal( str ) ;
					strvari1 = (char *)str.c_str() ;
					if( Stack.empty() ) {
	                    sprintf( errstr, "%Lf", vari2 ) ;

                        Str = strvari1  ;
                        Str += "="  ;
                        Str += errstr ;
/*
                        strVariables( Str ) ;
*/
						if( calcErrZero ){
							//0Š„‚ª”­¶
							SetVariable(str, DBL_MAX);
						}else{
							SetVariable(str, vari2);
						}

                        Str = strvari1 ;
                        Str += "=" ;
                        Str += errstr ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("strRpnOperation=%s\n", Rpn.c_str());
//	printf("rpnOperation()‚Ì’†‚Å=%f\n=%s\n", tTime, Str.c_str());
//#endif
                        return( Str ) ;
                    } else {
						throw RunError( "Syntax" ) ;
                    }
                }
			}
		}

	} catch( const RunError & e ) {
		sprintf( errmsg, "Error:%s", e ) ;
	} catch( ... ) {
    	strcpy( errmsg, "Error:Unkown Error!" ) ;
	}

    token = Stack.top().Str ;
    for( ; !Stack.empty() ; ) {
    	token = Stack.top().Str ;
        Stack.pop() ;
    }
    if( *errmsg == '\0' )
    	strcpy( errmsg, "Error:Syntax Error!" ) ;
	Str = token + "=0" ;
	strVariables( (char *)Str.c_str() ) ;

	Str = token + "=" + errmsg ;

    return( Str ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : rpnOperation
//
// INPUT OUTPUT :
//		char *		rpn					(I)	w’è‚³‚ê‚½®
//		char *		output				(I)	‰‰ZŒ‹‰Ê
//		int 		strErrorMessage[]	(I)	‰‰ZŒ‹‰Ê‚ğ¾¯Ä‚Æ‚·‚é»²½Ş
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		<rpn>‚Åw’è‚³‚ê‚½®‚ğ•]‰¿‚µ<output>‚Éo—Í
//----------------------------------------------------------------------------
void	CalcEngine::rpnOperation( const char *rpn, char *output, int size )
{
    string	result ;

double tTime = 0;
//Timer_Start();
	strRpnOperation = new char[strlen(rpn)+1] ;
	strcpy( strRpnOperation, rpn ) ;
	//®‚ğ•]‰¿‚µ‰‰ZÀs
	result = rpnOperation() ;
    strncpy( output, result.c_str(), size-1 ) ;
    delete[] strRpnOperation ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("rpnOperation()=%f\n", tTime);
//#endif
}

//----------------------------------------------------------------------------
// FUNCTION NAME : SetRpn
//
// INPUT OUTPUT :
//		string		Symbol				(I)	•Ï”–¼
//		double		strRpn				(I)	‰‰Z®
//		int			(RETURN)			(R)	Map“o˜^‚³‚ê‚½ŒÂ”
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		•Ï”–¼‚Æ‰‰Z®‚Ìİ’è
//----------------------------------------------------------------------------
//		’) “o˜^•Ï”–¼‚Æ‰‰Z®‚Í‘å•¶š‚É•ÏŠ·‚µ‚Ä‚ ‚é
//----------------------------------------------------------------------------
int CalcEngine::SetRpn(string Symbol, string strRpn)
{
	int	i = -1;
	map< string, string >::iterator mi ;
	string	name;
	string  rpn;

	//‘å•¶š‚É•ÏŠ·
	Tool_ChgUpper(Symbol, name); 
	Tool_ChgUpper(strRpn, rpn); 

	mi = rpnMap.find( name ) ;
	if( mi == rpnMap.end() || rpnMap.size() == 0 ) {
		//V‹K‚É“o˜^
		rpnMap.insert( make_pair( name, rpn ) ) ;   
	} else {
		//Šù‚É“o˜^‚³‚ê‚Ä‚¢‚é‚ªA®‚Ì‚İ‚ğ•ÏX‚·‚é
		(*mi).second = rpn;
	}
	
	i = rpnMap.size() ; 
    return( i ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : GetRpn
//
// INPUT OUTPUT :
//		string		Symbol				(I)	•Ï”–¼
//		double		strRpn				(I)	‰‰Z®
//		int			(RETURN)			(R)	–ß‚è’l(1F¬Œ÷A0F¸”s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		•Ï”–¼‚©‚ç‰‰Z®‚ğæ“¾‚·‚é
//----------------------------------------------------------------------------
int CalcEngine::GetRpn(string &Symbol, string &strRpn)
{
	string name("");

	//‘å•¶š‚É•ÏŠ·
	Tool_ChgUpper(Symbol, name); 
	Symbol = name;

    map< string, string >::iterator mi ;

        mi = rpnMap.find( name ) ;
        if( mi == rpnMap.end() || rpnMap.size() == 0 ) {
        	strRpn = "No Label";
			return 0;
        } else {
        	strRpn = (*mi).second ;
			return 1;
        }
}

//----------------------------------------------------------------------------
// FUNCTION NAME : init
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		ƒNƒŠƒA‚·‚é
//----------------------------------------------------------------------------
void CalcEngine::init(void)
{
    int n = vMap.size() ;
	if( n ){
		vMap.clear(); 
	}
	int k = rpnMap.size() ; 
	if( k ){
		rpnMap.clear(); 
	}

    for( ; !Stack.empty() ; ) {
    	Stack.pop() ;
    }

}

