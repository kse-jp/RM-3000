/////////////////////////////////////////////////////////////////////
//
//  CalcEngine.cpp
//  演算処理クラス
// 
//	2009-10-08	作成	中岡昌俊


#include "stdafx.h"

#pragma warning(disable:4786)

////////////////////////////////////////////////////////////////////////////
#pragma warning( disable : 4996 )
#define _CRT_SECURE_NO_DEPRECATE

#ifdef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#undef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1
//strcopyやsprintf()などの(バッファオーバラン対策)ワーニングを抑止する
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
//		string		str			(I)	文字情報の文字列
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		文字情報を確認し、値かﾗﾍﾞﾙ文字を登録する
//----------------------------------------------------------------------------
void CalcEngine::pushVal( string str )
{
	long double	val ;
    char	*s ;
    opStack	opr ;

	val = strtod( str.c_str(), &s ) ;	//文字列をdouble型の値にする
    if( *s ) {
    	opr.type = 0 ;
        opr.Str = str ;		//ﾗﾍﾞﾙとして登録
        opr.Val = 0 ;
    } else {
    	opr.type = 1 ;
        opr.Str = "" ;
        opr.Val = val ;		//値として登録
    }
    Stack.push( opr ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : pushVal
//
// INPUT OUTPUT :
//		long double		val		(I)	値
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		値として登録する
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
//		long double		(RETURN)		(R)	登録された値
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		ｽﾀｯｸとして保持された情報(ﾗﾍﾞﾙと値)から値として取得する
//----------------------------------------------------------------------------
//		注) 0割の情報なら0値を返す		
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
		// 変数(ﾗﾍﾞﾙ)定義されている
		mi = vMap.find( opr.Str ) ;
    	if( mi != vMap.end() ) {
        	val = (*mi).second ;
        } else {
			sprintf( errstr, "Undefined[%s]", opr.Str.c_str() ) ;

			throw RunError( errstr ) ;
        }
	} else {
		//値
	    val = opr.Val ;
    }

	if( val == DBL_MAX ){
		//0割の情報なら
		val = 0.0;
		calcErrZero = 1;
	}
    return( val ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : topVal
//
// INPUT OUTPUT :
//		string &		str		(O)	登録されたﾗﾍﾞﾙ文字列
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		ｽﾀｯｸとして保持された情報からﾗﾍﾞﾙを取得する
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
//		long double		(RETURN)		(R)	登録された値
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		ｽﾀｯｸとして保持された情報(ﾗﾍﾞﾙと値)から値として取得しｽﾀｯｸに積む
//----------------------------------------------------------------------------
//		注) 0割の情報なら0値を返す		
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
//		string &		str		(O)	登録されたﾗﾍﾞﾙ文字列
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		ｽﾀｯｸとして保持された情報からﾗﾍﾞﾙを取得する
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
//		string		variable			(I)	登録する変数名
//		int			(RETURN)			(R)	登録した個数
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変数名と変数値の設定
//		"\r\n"で区切られた複数の設定が可能
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
//		char *		variable			(O)	登録されている変数名
//		int			size				(I)	ｾｯﾄするﾊﾞｯﾌｧｻｲｽﾞ
//		int			(RETURN)			(R)	登録した個数
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		登録されている変数名の取得
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
//		char *		variable			(I)	変換する文字列
//		long double	(RETURN)			(R)	取得値
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		数字文字列から値に変換
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
//		char *		line				(I)	論理式(比較演算子)の文字列
//		long double	vari1				(I)	比較する値1
//		long double	vari2				(I)	比較する値2
//		long double	(RETURN)			(R)	論理式(比較)の結果(1/0)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		論理式(比較演算子)の処理
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
//		long double	vari2				(I)	演算する値
//		long double	(RETURN)			(R)	演算結果
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		関数の処理
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
		//	２項演算子
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
			//	３項演算子
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
//		int			theOp				(I)	算術計算記号
//		long double	vari1				(I)	変数値
//		long double	vari2				(I)	変数値
//		long double		(RETURN)		(R)	演算計算値
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		算術計算の処理
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
								calcErrZero = 1;	//0割
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
//		string		strVariableName		(I)	登録する変数名
//		double		valVariableVal		(I)	登録する変数値
//		int			(RETURN)			(R)	Map登録された個数
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変数名と変数値の設定
//----------------------------------------------------------------------------
//		注) 登録変数名は大文字に変換してある
//----------------------------------------------------------------------------
int CalcEngine::SetVariable(string strVariableName, double valVariableVal)
{
	int	i = -1;
	map< string, long double >::iterator mi ;
	string	name;
	long double	v = valVariableVal;

	//大文字に変換
	Tool_ChgUpper(strVariableName, name); 

	mi = vMap.find( name ) ;
	if( mi == vMap.end() || vMap.size() == 0 ) {
		//新規に登録
		vMap.insert( make_pair( name, v ) ) ;   
	} else {
		//既に登録されているが、値のみを変更する
		(*mi).second = v ;
	}
	
	i = vMap.size() ; 
    return( i ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : isSymbol
//
// INPUT OUTPUT :
//		string		symbol				(I)	変数名
//		int			(RETURN)			(R)	登録済みﾁｪｯｸ値(true：登録済み、false：登録されてない)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変数の登録済みチェック
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
//		string			symbol		(I)	変数名
//		long double		val			(I)	登録する値
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変数の登録／変更
//		新規の場合は登録、登録済みの場合は値の更新を行う
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
//		string			symbol		(I)	変数名
//		long double		(RETURN)	(R)	演算計算値
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変数の取得
//----------------------------------------------------------------------------
long double	CalcEngine::getSymbol( string symbol )
{
	string name("");
	long double fVal;

	//大文字に変換
	Tool_ChgUpper(symbol, name); 

    map< string, long double >::iterator mi ;

        mi = vMap.find( name ) ;
        if( mi == vMap.end() || vMap.size() == 0 ) {
        	return 0 ;
        } else {
			fVal = (*mi).second ;
			if( fVal == DBL_MAX ){
				//0割りした結果
				fVal = 0.0;
			}
        	return fVal;
        }
}

//----------------------------------------------------------------------------
// FUNCTION NAME : rpnOperation
//
// INPUT OUTPUT :
//		string		(RETURN)			(R)	演算結果(演算変数名=演算値)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		演算処理
//		strRpnOperationにｾｯﾄされた演算式を評価して演算を実行する
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

	// 計算
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

			// 演算子かどうかの判定
			c = token[0] ;
			if( (c != '+'  && c != '-' && c != '.' && c != '*' && c != '/' && c != '%' && c != '^' && c != '=' && c != '@')) {
				// 値ならばスタックにpush
//Timer_Start();
                pushVal( token ) ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("pushVal()で=%f\n", tTime);
//#endif
			} else {
				//演算子
				if(c == '@') {
//Timer_Start();
	                vari2 = popVal() ;
					vari0 = Function( vari2 ) ;
                    pushVal( vari0 ) ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("@ popVal/pushVal()で=%f\n", tTime);
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
								//0割が発生したﾃﾞｰﾀを使用した
	                        	pushVal( 0.0 ) ;
							}else{
//double tTime = 0;
//Timer_Start();
								vari = Arithmetic( c, vari1, vari2 ) ;
	                        	pushVal( vari ) ;
//Timer_Stop(&tTime);
//#ifdef DEBUG_PRINT5
//	printf("Arithmetic()で=%f\n", tTime);
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
							//0割が発生
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
//	printf("rpnOperation()の中で=%f\n=%s\n", tTime, Str.c_str());
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
//		char *		rpn					(I)	指定された式
//		char *		output				(I)	演算結果
//		int 		strErrorMessage[]	(I)	演算結果をｾｯﾄとするｻｲｽﾞ
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		<rpn>で指定された式を評価し<output>に出力
//----------------------------------------------------------------------------
void	CalcEngine::rpnOperation( const char *rpn, char *output, int size )
{
    string	result ;

double tTime = 0;
//Timer_Start();
	strRpnOperation = new char[strlen(rpn)+1] ;
	strcpy( strRpnOperation, rpn ) ;
	//式を評価し演算実行
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
//		string		Symbol				(I)	変数名
//		double		strRpn				(I)	演算式
//		int			(RETURN)			(R)	Map登録された個数
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変数名と演算式の設定
//----------------------------------------------------------------------------
//		注) 登録変数名と演算式は大文字に変換してある
//----------------------------------------------------------------------------
int CalcEngine::SetRpn(string Symbol, string strRpn)
{
	int	i = -1;
	map< string, string >::iterator mi ;
	string	name;
	string  rpn;

	//大文字に変換
	Tool_ChgUpper(Symbol, name); 
	Tool_ChgUpper(strRpn, rpn); 

	mi = rpnMap.find( name ) ;
	if( mi == rpnMap.end() || rpnMap.size() == 0 ) {
		//新規に登録
		rpnMap.insert( make_pair( name, rpn ) ) ;   
	} else {
		//既に登録されているが、式のみを変更する
		(*mi).second = rpn;
	}
	
	i = rpnMap.size() ; 
    return( i ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : GetRpn
//
// INPUT OUTPUT :
//		string		Symbol				(I)	変数名
//		double		strRpn				(I)	演算式
//		int			(RETURN)			(R)	戻り値(1：成功、0：失敗)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変数名から演算式を取得する
//----------------------------------------------------------------------------
int CalcEngine::GetRpn(string &Symbol, string &strRpn)
{
	string name("");

	//大文字に変換
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
//		クリアする
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

