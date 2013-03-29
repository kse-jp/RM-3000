/////////////////////////////////////////////////////////////////////
//
//  CalcEngine.cpp
//  ���Z�����N���X
// 
//	2009-10-08	�쐬	�������r


#include "stdafx.h"

#pragma warning(disable:4786)

////////////////////////////////////////////////////////////////////////////
#pragma warning( disable : 4996 )
#define _CRT_SECURE_NO_DEPRECATE

#ifdef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#undef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1
//strcopy��sprintf()�Ȃǂ�(�o�b�t�@�I�[�o�����΍�)���[�j���O��}�~����
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
//		string		str			(I)	�������̕�����
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		���������m�F���A�l�����ٕ�����o�^����
//----------------------------------------------------------------------------
void CalcEngine::pushVal( string str )
{
	long double	val ;
    char	*s ;
    opStack	opr ;

	val = strtod( str.c_str(), &s ) ;	//�������double�^�̒l�ɂ���
    if( *s ) {
    	opr.type = 0 ;
        opr.Str = str ;		//���قƂ��ēo�^
        opr.Val = 0 ;
    } else {
    	opr.type = 1 ;
        opr.Str = "" ;
        opr.Val = val ;		//�l�Ƃ��ēo�^
    }
    Stack.push( opr ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : pushVal
//
// INPUT OUTPUT :
//		long double		val		(I)	�l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�l�Ƃ��ēo�^����
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
//		long double		(RETURN)		(R)	�o�^���ꂽ�l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�����Ƃ��ĕێ����ꂽ���(���قƒl)����l�Ƃ��Ď擾����
//----------------------------------------------------------------------------
//		��) 0���̏��Ȃ�0�l��Ԃ�		
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
		// �ϐ�(����)��`����Ă���
		mi = vMap.find( opr.Str ) ;
    	if( mi != vMap.end() ) {
        	val = (*mi).second ;
        } else {
			sprintf( errstr, "Undefined[%s]", opr.Str.c_str() ) ;

			throw RunError( errstr ) ;
        }
	} else {
		//�l
	    val = opr.Val ;
    }

	if( val == DBL_MAX ){
		//0���̏��Ȃ�
		val = 0.0;
		calcErrZero = 1;
	}
    return( val ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : topVal
//
// INPUT OUTPUT :
//		string &		str		(O)	�o�^���ꂽ���ٕ�����
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�����Ƃ��ĕێ����ꂽ��񂩂����ق��擾����
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
//		long double		(RETURN)		(R)	�o�^���ꂽ�l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�����Ƃ��ĕێ����ꂽ���(���قƒl)����l�Ƃ��Ď擾�������ɐς�
//----------------------------------------------------------------------------
//		��) 0���̏��Ȃ�0�l��Ԃ�		
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
//		string &		str		(O)	�o�^���ꂽ���ٕ�����
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�����Ƃ��ĕێ����ꂽ��񂩂����ق��擾����
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
//		string		variable			(I)	�o�^����ϐ���
//		int			(RETURN)			(R)	�o�^������
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϐ����ƕϐ��l�̐ݒ�
//		"\r\n"�ŋ�؂�ꂽ�����̐ݒ肪�\
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
//		char *		variable			(O)	�o�^����Ă���ϐ���
//		int			size				(I)	��Ă����ޯ̧����
//		int			(RETURN)			(R)	�o�^������
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�o�^����Ă���ϐ����̎擾
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
//		char *		variable			(I)	�ϊ����镶����
//		long double	(RETURN)			(R)	�擾�l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		���������񂩂�l�ɕϊ�
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
//		char *		line				(I)	�_����(��r���Z�q)�̕�����
//		long double	vari1				(I)	��r����l1
//		long double	vari2				(I)	��r����l2
//		long double	(RETURN)			(R)	�_����(��r)�̌���(1/0)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�_����(��r���Z�q)�̏���
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
//		long double	vari2				(I)	���Z����l
//		long double	(RETURN)			(R)	���Z����
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�֐��̏���
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
		//	�Q�����Z�q
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
			//	�R�����Z�q
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
//		int			theOp				(I)	�Z�p�v�Z�L��
//		long double	vari1				(I)	�ϐ��l
//		long double	vari2				(I)	�ϐ��l
//		long double		(RETURN)		(R)	���Z�v�Z�l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�Z�p�v�Z�̏���
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
								calcErrZero = 1;	//0��
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
//		string		strVariableName		(I)	�o�^����ϐ���
//		double		valVariableVal		(I)	�o�^����ϐ��l
//		int			(RETURN)			(R)	Map�o�^���ꂽ��
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϐ����ƕϐ��l�̐ݒ�
//----------------------------------------------------------------------------
//		��) �o�^�ϐ����͑啶���ɕϊ����Ă���
//----------------------------------------------------------------------------
int CalcEngine::SetVariable(string strVariableName, double valVariableVal)
{
	int	i = -1;
	map< string, long double >::iterator mi ;
	string	name;
	long double	v = valVariableVal;

	//�啶���ɕϊ�
	Tool_ChgUpper(strVariableName, name); 

	mi = vMap.find( name ) ;
	if( mi == vMap.end() || vMap.size() == 0 ) {
		//�V�K�ɓo�^
		vMap.insert( make_pair( name, v ) ) ;   
	} else {
		//���ɓo�^����Ă��邪�A�l�݂̂�ύX����
		(*mi).second = v ;
	}
	
	i = vMap.size() ; 
    return( i ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : isSymbol
//
// INPUT OUTPUT :
//		string		symbol				(I)	�ϐ���
//		int			(RETURN)			(R)	�o�^�ς������l(true�F�o�^�ς݁Afalse�F�o�^����ĂȂ�)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϐ��̓o�^�ς݃`�F�b�N
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
//		string			symbol		(I)	�ϐ���
//		long double		val			(I)	�o�^����l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϐ��̓o�^�^�ύX
//		�V�K�̏ꍇ�͓o�^�A�o�^�ς݂̏ꍇ�͒l�̍X�V���s��
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
//		string			symbol		(I)	�ϐ���
//		long double		(RETURN)	(R)	���Z�v�Z�l
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϐ��̎擾
//----------------------------------------------------------------------------
long double	CalcEngine::getSymbol( string symbol )
{
	string name("");
	long double fVal;

	//�啶���ɕϊ�
	Tool_ChgUpper(symbol, name); 

    map< string, long double >::iterator mi ;

        mi = vMap.find( name ) ;
        if( mi == vMap.end() || vMap.size() == 0 ) {
        	return 0 ;
        } else {
			fVal = (*mi).second ;
			if( fVal == DBL_MAX ){
				//0���肵������
				fVal = 0.0;
			}
        	return fVal;
        }
}

//----------------------------------------------------------------------------
// FUNCTION NAME : rpnOperation
//
// INPUT OUTPUT :
//		string		(RETURN)			(R)	���Z����(���Z�ϐ���=���Z�l)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		���Z����
//		strRpnOperation�ɾ�Ă��ꂽ���Z����]�����ĉ��Z�����s����
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

	// �v�Z
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

			// ���Z�q���ǂ����̔���
			c = token[0] ;
			if( (c != '+'  && c != '-' && c != '.' && c != '*' && c != '/' && c != '%' && c != '^' && c != '=' && c != '@')) {
				// �l�Ȃ�΃X�^�b�N��push
//Timer_Start();
                pushVal( token ) ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("pushVal()��=%f\n", tTime);
//#endif
			} else {
				//���Z�q
				if(c == '@') {
//Timer_Start();
	                vari2 = popVal() ;
					vari0 = Function( vari2 ) ;
                    pushVal( vari0 ) ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("@ popVal/pushVal()��=%f\n", tTime);
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
								//0�������������ް����g�p����
	                        	pushVal( 0.0 ) ;
							}else{
//double tTime = 0;
//Timer_Start();
								vari = Arithmetic( c, vari1, vari2 ) ;
	                        	pushVal( vari ) ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("Arithmetic()��=%f\n", tTime);
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
							//0��������
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
//	printf("rpnOperation()�̒���=%f\n=%s\n", tTime, Str.c_str());
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
//		char *		rpn					(I)	�w�肳�ꂽ��
//		char *		output				(I)	���Z����
//		int 		strErrorMessage[]	(I)	���Z���ʂ�ĂƂ��黲��
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		<rpn>�Ŏw�肳�ꂽ����]����<output>�ɏo��
//----------------------------------------------------------------------------
void	CalcEngine::rpnOperation( const char *rpn, char *output, int size )
{
    string	result ;

double tTime = 0;
//Timer_Start();
	strRpnOperation = new char[strlen(rpn)+1] ;
	strcpy( strRpnOperation, rpn ) ;
	//����]�������Z���s
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
//		string		Symbol				(I)	�ϐ���
//		double		strRpn				(I)	���Z��
//		int			(RETURN)			(R)	Map�o�^���ꂽ��
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϐ����Ɖ��Z���̐ݒ�
//----------------------------------------------------------------------------
//		��) �o�^�ϐ����Ɖ��Z���͑啶���ɕϊ����Ă���
//----------------------------------------------------------------------------
int CalcEngine::SetRpn(string Symbol, string strRpn)
{
	int	i = -1;
	map< string, string >::iterator mi ;
	string	name;
	string  rpn;

	//�啶���ɕϊ�
	Tool_ChgUpper(Symbol, name); 
	Tool_ChgUpper(strRpn, rpn); 

	mi = rpnMap.find( name ) ;
	if( mi == rpnMap.end() || rpnMap.size() == 0 ) {
		//�V�K�ɓo�^
		rpnMap.insert( make_pair( name, rpn ) ) ;   
	} else {
		//���ɓo�^����Ă��邪�A���݂̂�ύX����
		(*mi).second = rpn;
	}
	
	i = rpnMap.size() ; 
    return( i ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : GetRpn
//
// INPUT OUTPUT :
//		string		Symbol				(I)	�ϐ���
//		double		strRpn				(I)	���Z��
//		int			(RETURN)			(R)	�߂�l(1�F�����A0�F���s)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϐ������牉�Z�����擾����
//----------------------------------------------------------------------------
int CalcEngine::GetRpn(string &Symbol, string &strRpn)
{
	string name("");

	//�啶���ɕϊ�
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
//		�N���A����
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

