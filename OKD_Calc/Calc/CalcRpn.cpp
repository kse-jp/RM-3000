/////////////////////////////////////////////////////////////////////
//
//  CalcRpn.cpp
//  演算式の解析クラス
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
#include <string.h>
#include <string>

#include <stack>
#include <map>
#include <math.h>

using namespace std;

#include "Tool.h"
#include "CalcRpn.h"
#include "CalcFunc.h"
#include "CalcMathErr.h"
#include "CalcEngine.h"


#define  Sign       0x80
#define  Plus       ('+'+Sign)
#define  Minus      ('-'+Sign)

CalcFunc    *fTable = new CalcFunc() ;
struct rpnErr   eRpn ;
extern CalcEngine	Calc;

//----------------------------------------------------------------------------
// FUNCTION NAME : putSpace
//
// INPUT OUTPUT :
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		1つのｽﾍﾟｰｽを追加する
//		変数が２文字以上でも区別できるように
//----------------------------------------------------------------------------
void	CalcRpn::putSpace( void )
{
	rpn += ' ' ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : putOp
//
// INPUT OUTPUT :
//		int			theOp				(I)	
//		int			(RETURN)			(R)	
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		
//----------------------------------------------------------------------------
int	CalcRpn::putOp( int theOp )
{
	string	term ;

    term = opStack.top() ;
    if( !opStack.empty() )
    	opStack.pop() ;

    if( term[0] & Sign ) {
        term[0] = term[0] - Sign ;
    }
    rpn += term ;
		if( opStack.empty() )
        	cp_stack = '\0' ;
        else
			cp_stack = opStack.top()[0] ;
    return( theOp ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : opPriority
//
// INPUT OUTPUT :
//		int			theOp				(I)	
//		int			(RETURN)			(R)	
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		
//----------------------------------------------------------------------------
int	CalcRpn::opPriority( int theOp )
{
	int		level ;

	switch( theOp ) {
		case '=':
        case '(':
			level = 0 ;
            break ;
        case '.':
			level = 1 ;
            break ;
        case '+':
        case '-':
			level = 2 ;
            break ;
		case '*':
        case '/':
        case '%':
			level = 3;
			break ;
        case '^':
			level = 4;
			break ;
        case Plus:
        case Minus:
			level = 10;
			break ;
		default:			//	'@'
			level = 5;
            break ;
    }
    return( level ) ;

}

//----------------------------------------------------------------------------
// FUNCTION NAME : opStackPush
//
// INPUT OUTPUT :
//		int			theOp				(I)	
//		int			(RETURN)			(R)	
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		
//----------------------------------------------------------------------------
void	CalcRpn::opStackPush( int theOp )
{
	string	term = "" ;

    term += (char)theOp ;
    if( theOp == '.' ) {
		term += *p_input++ ;
		term += *p_input++ ;
    }
    opStack.push( term ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : processOp
//
// INPUT OUTPUT :
//		int			theOp				(I)	
//		int			(RETURN)			(R)	
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		
//----------------------------------------------------------------------------
void	CalcRpn::processOp( int theOp )
{
	int		level_input;					// cp_input の優先順位
	int		level_stack;					// cp_stack の優先順位

	if(cp_stack == '\0' || cp_stack == '(') {
    	opStackPush( theOp ) ;
        putSpace() ;
        return ;
	}

    //優先順位を設定
    if( theOp == '-' || theOp == '+' ) {	// 単項（符号）の処理
		if( sign == 1 ){		//true ) {
            theOp += Sign ;
        }
    }
    level_input = opPriority( theOp ) ;
	for( ; ; ) {
    	level_stack = opPriority( cp_stack ) ;
		if(level_input <= level_stack) {
        	putSpace() ;
            putOp( cp_stack ) ;
		} else {
    		break ;
	    }
	}

   	putSpace() ;
    opStackPush( theOp ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : opAllPop
//
// INPUT OUTPUT :
//		int			theOp				(I)	
//		int			(RETURN)			(R)	
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		
//----------------------------------------------------------------------------
int	CalcRpn::opAllPop( int theOp )
{
	for( ; ; ) {
		if(cp_stack == '(' || cp_stack == ',' ) {
        	if( theOp != ',' )
				opStack.pop() ;
        	return true ;
		}

       	putSpace() ;
        if( putOp( cp_stack ) == '=') {
        	return false ;
        }
	}
}

//----------------------------------------------------------------------------
// FUNCTION NAME : opAllPop
//
// INPUT OUTPUT :
//		int			theOp				(I)	
//		int			(RETURN)			(R)	
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		演算子か否かのチェック
//----------------------------------------------------------------------------
int	CalcRpn::isOperators( int theOp )
{
	switch( theOp ) {
    	case '+':
        case '-':
        case '*':
        case '/':
        case '%':
        case '^':
        	return opCharStd ;
        case '@':
        	return opCharFunc ;
        case '.':
        case '=':
        case '>':
        case '<':
        	return opCharLog ;
        case '(':
        	return opCharLparen ;
        case ')':
        	return opCharRparen ;
        case ',':
        	return opCharComma ;
	}
	return opCharNo ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : opAllPop
//
// INPUT OUTPUT :
//		string		func				(I)	検索文字列
//		int			start				(I)	検索開始位置
//		int			(RETURN)			(R)	')'の位置(ｲﾝﾃﾞｯｸｽ値)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		対応する')'の検索
//----------------------------------------------------------------------------
static int	EndFunc( string func, int start )
{
	int		node ;
    int		s ;
    int		_s ;

    s = start ;
    _s = strlen( func.c_str() ) ;
    for( node = 0 ; s < _s ; s++) {
        if( func[s] == '(' ) {
            node++ ;
        } else if( func[s] == ')' ) {
        	if( node == 0 ) {
				return( s ) ;
            } else {
                node-- ;
            }
        }
    }
    return( -1 ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : FuncCheck
//
// INPUT OUTPUT :
//		string &	strin				(I) 評価する演算式の文字列
//		int			(RETURN)			(R)	結果
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		関数かﾁｪｯｸする
//		関数の'('の前に'@'を挿入されている
//		比較演算子を'.xx'に変換
//----------------------------------------------------------------------------
static int FuncCheck( string &strin )
{
    string  func ;
    int     s ;
    int     n ;
    int     i ;
    string  name ;
    map< string, int >::iterator mi ;
    string  nest ;
    int     first ;
    int     last ;
    string	remain ;

    func = strin ;
    s = func.find( "@(" ) ;
    if( s < 0 )
    	return( 0 ) ;

	s += 2 ;
	s = EndFunc( func, s ) ;

    if( s < 0 ) {
        eRpn.term = func ;
        throw eErrRparenthesis ;
    }
    remain = func ;
    remain.erase( 0, s+1 ) ;
    func.erase( s+1, func.size() ) ;


    //  関数名のチェック
    n = func.find( "@(" ) ;
    name = func ;
    name.erase( n, func.size() ) ;
    for( i = 0 ; i < n ; i++ ) {
    	mi = fTable->fMap.find( name ) ;
		if( mi == fTable->fMap.end() ) {
        	name.erase( 0, 1 ) ;
            continue ;
		} else {
        	break ;
		}
	}
    if( i >= n ) {
        eRpn.term = func ;
        throw eErrFunction ;
    }
    func.erase( 0, i ) ;

    //  パラメータ数のチェック
    name = func ;
    n = func.find( "@(" ) ;
    name.erase( 0, n+2 ) ;
	n = EndFunc( name, 0 ) ;
    name.erase( n, name.size() ) ;

    n = FuncCheck( remain ) ;
    if( n != eErrOK )
        return( n ) ;

    n = name.find( "@(" ) ;
    if( n >= 0 ) {
        nest = name ;
        n = FuncCheck( nest ) ;
        if( n != 0 ) {
            return( n ) ;
        }
        first = name.find( "@(" ) ;
        last = EndFunc( name, first+2 ) ;

        name.erase( first, last-first+1 ) ;
        name.insert( first, "done" ) ;
    }

    if( (*mi).second == 0 ) {
        if( ( i = name.find( "," ) ) >= 0 ) {
            eRpn.term = func ;
            throw eErrExtraParameter ;
        }
        i = strlen( name.c_str() ) ;
        if( i != 0 ) {
            eRpn.term = func ;
            throw eErrExtraParameter ;
        }
    } else if( (*mi).second == 1 ) {
        if( ( i = name.find( "," ) ) >= 0 ) {
            eRpn.term = func ;
            throw eErrExtraParameter ;
        }
        i = strlen( name.c_str() ) ;
        if( i == 0 ) {
            eRpn.term = func ;
            throw eErrTooFewParameter ;
        }
    } else {
        if( ( i = name.find( "," ) ) < 0 ) {
            eRpn.term = func ;
            throw eErrTooFewParameter ;
        }
        for( ; ; ) {
            first = name.find( "(" ) ;
	        last = EndFunc( name, first+1 ) ;
            if( first > 0 && last > 0 ) {
                name.erase( first, last-first+1 ) ;
            } else {
                break ;
            }
        }

		n = stringToken(name, ",");

        if( n < (*mi).second ) {
            eRpn.term = func ;
            throw eErrTooFewParameter ;
        } else if( n > (*mi).second ) {
            eRpn.term = func ;
            throw eErrExtraParameter ;
        }

    }
    return( eErrOK ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : preProcess
//
// INPUT OUTPUT :
//		string &	strin				(I) 文字列
//		string &	strout				(O) 文字列
//		int			(RETURN)			(R)	
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		
//----------------------------------------------------------------------------
int CalcRpn::preProcess( string &strin, string &strout )
{
	int		i ;
    int		flag = 0 ;
    int		c ;
    int		size ;
    string  temp ;
    string  str ;
    int     s ;

    try{

    	for( ; strout.size() > 0 ; )
	    	strout.erase() ;
    	// 関数には '@' を挿入
    	size = strin.size() ;
	    for( i = 0 ; i < size ; ) {
    	    c = strin[i++] ;
    		if(c == '(') {
            	c = strin[i-2] ;
            	if( ! isOperators( c ) ) {
			    	//'('の手前の文字が演算子用の予約文字でない場合に関数と判断する
				    strout += '@';
                }
			    strout += '(';
    		} else if( c == '=' ) {
            	if( flag == 0 ) {
                	flag = 1 ;
			    	strout += (char)c;
                } else {
				    strout += ".EQ" ;
                }
	    	} else if( c == '>' ) {
            	if( strin[i] == '=' ) {
                	i++ ;
				    strout += ".GE" ;
                } else {
	    			strout += ".GT" ;
                }
    		} else if( c == '<' ) {
            	if( strin[i] == '>' ) {
                	i++ ;
			    	strout += ".NE" ;
            	} else if( strin[i] == '=' ) {
                	i++ ;
		    		strout += ".LE" ;
                } else {
				    strout += ".LT" ;
                }
	    	} else if( c == '*' ) {
            	if( strin[i] == '*' ) {
			    	strout += '^' ;
            	    i++ ;
                } else {
	    			strout += (char)c ;
                }
    		} else {
	    		strout += (char)c ;
            }
    	}
	    strout += '\0';

    //  Check
        temp = strout ;
        eRpn.term = "" ;
        if( temp.find( "=" ) < 0 ) {
            throw eErrAssinmentOperator ;
        }

		i = stringToken(temp, "=");
		std::vector< string > ss( i ) ;
		for(int ii = 0; ii< i; ii++){
			ss.at(ii) = strTokenBuf[ii];
		}
		if( i <= 1 ){
            throw eErrAssinmentOperator ;
		}else{
			if( ss.at(0).empty() ){
	            throw eErrNoExp ;
			}
			str = ss.at(0);

			if( ss.at(1).empty() ){
	            throw eErrNoExp ;
			}
			str = ss.at(1);
		}

        //  かっこの対応
        for( s = 0, i = 0 ;  ; i++ ) {
            s = str.find( "(", s ) ;
            if( s < 0 )
                break ;
            s++ ;
        }
        for( s = 0 ;  ; i-- ) {
            s = str.find( ")", s ) ;
            if( s < 0 )
                break ;
            s++ ;
        }
        if( i != 0 ) {
            eRpn.term = str ;
            if( i > 0 )
                throw eErrRparenthesis ;
            else
                throw eErrLparenthesis ;
        }

        //  関数のパラメータ数
        if( FuncCheck( str ) ) {
        }

    } catch( enum errExpression e ) {
        eRpn.code = e ;
        strout = errMsg() ;
        return( e ) ;
    } catch(...) {
        eRpn.code = eErrSyntax ;
        eRpn.term = "<Other errors>" ;
        strout = errMsg() ;
        return( eErrSyntax ) ;
    }

    return( eErrOK ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : strRPN
//
// INPUT OUTPUT :
//		char *		input				(I) 文字列
//		char *		output				(O) 文字列
//		int 		size				(I) ﾊﾞｯﾌｧｻｲｽﾞ
//		int			(RETURN)			(R)	成功：0  失敗:1	
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		
//----------------------------------------------------------------------------
//	スタックによる数式のＲＰＮへの変換の際のスタック操作規則		
//	（関数も表現できるように改良。また、剰余や正負号にも対応）      
//	L0:     ０）一文字読み込み                                      
//	L1:     １）変数、定数はそのまま出力                            
//	L2:     ２）演算子(op1)											
//	           Ⅰ）スタックが空、またはスタックの先頭が'('のとき、  
//					op1をpush                                       
//	           Ⅱ）スタックの先頭が、演算子(op2)なら                
//	               op1とop2の優先順位を比べる						
//	               ａ） (op1の優先順位) ≦ (op2の優先順位)          
//	                      op2をpop                                  
//	               ｂ） (op1の優先順位) ≧ (op2の優先順位)          
//	                      op1をpush                                 
//	L3:     ３）'('はpush                                           
//	L4:     ４）')'または、文字列の終わり                           
//	           Ⅰ）スタックの先頭が'('のとき、()を渡す              
//	           Ⅱ）スタックの先頭が演算子(op2)なら                  
//	               ａ)  op2をpop                                    
//	               ｂ） op2が'='なら、popして終了                   
//                                                                
//	"f(x)" のような形式を一度、"f@(x)" のように変換して、			
//		'@'が関数呼び出しであると仮定する                           
//	op の優先順位                                                   
//	  3 : '@'                                                       
//	  2 : '*', '/', '%' (余り)                                      
//	  1 : '+', '-'                                                  
//	  0 : '='                                                       
//----------------------------------------------------------------------------
//	使用制限：
//		代入演算子(assignment operator)が無い場合エラー
//		左辺値に演算子、'@'は使用できない
//----------------------------------------------------------------------------
int CalcRpn::strRPN(const char *input, char *output, int size )
{
	int		i ;
	int		c;
	string	strtemp( "" ) ;		// 一時的に格納する
	string	strout( "" ) ;
	char	cp_input;			// *p_input を格納
	int		flag = 0;     		// '='の有無
    char    *p ;
	char	tmp[512];

	//大文字に変換
	Tool_ChgUpperChar(input, tmp); 

    for( ; ; ) {
    	if( opStack.empty() )
        	break ;
        opStack.pop() ;
    }
	for( ; rpn.size() > 0 ; )
		rpn.erase() ;

// エラー処理    特に '=' がないと致命的なエラーになるから
	i = 0;

	while((c = tmp[i]) != '\0') {
		if(c == '=') {
			flag = 1;
			break;
		}
//ADD 2009-11-17
		if( !flag && c == '.' ){		//"PHASE_DIST_SEL2.1="対応
			goto NEXT_AAA;
		}
//ADD 2009-11-17
		if( ( isOperators( c ) == opCharFunc ) || ( isOperators( c ) == opCharLog )
        		|| (!flag && ( isOperators( c ) == opCharStd ) ) ) {

			//	左辺値に演算子、'@'は使用できない
            eRpn.code = eErrLvalue ;
            strout = errMsg() ;
            strncpy( output, strout.c_str(), size-1 ) ;
			return eErrLvalue ;
		}
NEXT_AAA:
		i++;
	}
	if(i == 0 || flag == 0) {
        eRpn.code = eErrAssinmentOperator ;
        strout = errMsg() ;
        strncpy( output, strout.c_str(), size-1 ) ;
		return eErrAssinmentOperator ;
	}

//*******************************

	for( ; strtemp.size() > 0 ; )
		strtemp.erase() ;
	i = 0;       // スペースを除去
	while((c=tmp[i++]) != '\0') {
		if(c != ' ') {
			strtemp += (char)c;
		}
	}
	strtemp += '\0';

	if( ( i = preProcess( strtemp, strout ) ) != eErrOK ) {
		strncpy(output, strout.c_str(), size-1 ) ;
        return i ;
    }


    p_input = (char *)strout.c_str() ;

	//	処理ループ
    sign = 1;		//true ;
	cp_stack = '\0' ;
	for( ; ; ) {
		cp_input = *p_input;
		if(strcmp(p_input, "") != 0) {
			p_input++;
		}

        //
		if( opStack.empty() )
        	cp_stack = '\0' ;
        else
			cp_stack = opStack.top()[0] ;

		if(cp_input == '+' || cp_input == '-' || cp_input == '*' || cp_input == '/' || cp_input == '^'
        		|| ( cp_input == '.' && !isdigit(*p_input) )
    			|| cp_input == '%' || cp_input == '=' || cp_input == '@') {

			processOp( cp_input ) ;

    		sign = isOperators( cp_input ) ;
        	if( sign == opCharStd || sign == opCharLog ) {
        		sign = 1;	//true ;
            } else {
            	sign = 0;	//false  ;
            }

		} else if (cp_input == '(') {
			opStackPush( cp_input ) ;
            sign = 0;		//false  ;
		} else if( cp_input == ')' ) {
			if( opAllPop( cp_input ) == false ) {
				strcpy(output, "Expression Syntax!");
				return eErrSyntax ;
			}
		} else if( cp_input == '\0' ) {
			if( opAllPop( cp_input ) == false )
        		break  ;
			strcpy(output, "Expression Syntax!");
			return eErrSyntax ;
		} else if( cp_input == ',' ) {
			if( opAllPop( cp_input ) == false ) {
				strcpy(output, "Expression Syntax!");
				return eErrSyntax ;
			}

            putSpace() ;
            sign = 0;		//false  ;
		} else {
            if( (sign) && isdigit( cp_input ) && ( cp_input != '.') ) {

			    strtod( &p_input[-1], &p ) ;
                for( ; ; ) {
                    rpn += cp_input ;
                    if( p_input >= p )
                        break ;
		            cp_input = *p_input;
		            if(strcmp(p_input, "") != 0) {
			            p_input++;
		            } else {
                        break ;
                    }
                }
            } else {
    			rpn += cp_input ;
            }




            sign = 0;		//false  ;
    	}
	}

	//	関数のパラメータの区切り','を’'に置換
	for( ; ; ) {
    	i = rpn.find( ',' ) ;
        if( i >= 0 )
        	rpn[i] = ' ' ;
        else
        	break ;
    }

	// 正負号に対応させるため
	// -d を 0-d という意味にする
    for( ; ; ) {
    	i = rpn.find( "  " ) ;
        if( i < 0 )
        	break ;
        rpn.insert( i+1, "0" ) ;
    }
//basic_string
    strncpy( output, rpn.c_str(), size-1 ) ;

    eRpn.code = eErrSyntax ;
    eRpn.term = "" ;
    try{

	    checkRPN( rpn ) ;
        checkItem( rpn ) ;

    } catch( enum errExpression e ) {
        eRpn.code = e ;
        strout = errMsg() ;
        strncpy( output, strout.c_str(), size-1 ) ;
        return( e ) ;
    } catch( enum errWarning e ) {
        eRpn.code = e ;
        strout = rpn + "\r\n" + errMsg() ;
        strncpy( output, strout.c_str(), size-1 ) ;
        return( e ) ;
    } catch(...) {
        eRpn.code = eErrSyntax ;
        strout = errMsg() ;
        strncpy( output, strout.c_str(), size-1 ) ;
        return( eErrSyntax ) ;
    }

	return eErrOK ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : checkItem
//
// INPUT OUTPUT :
//		string		rpn					(I)	変換された式の文字列
//		int			(RETURN)			(R)	0:正常
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		変換された式の評価(ﾗﾍﾞﾙﾁｪｯｸ)
//----------------------------------------------------------------------------
int CalcRpn::checkItem( string rpn )
{
    unsigned int         i ;
    char        *p ;
    long double v ;
    string      item ;
    map< string, int >::iterator mi ;
    map< string, long double >::iterator mv ;

	char tt[1024];

	strcpy(tt, rpn.c_str()); 

	unsigned int k = stringToken(rpn, " ");
	std::vector< string > ss( k ) ;
	for(unsigned int ii = 0; ii< k; ii++){
		ss.at(ii) = strTokenBuf[ii];
	}
    for( i = 0 ; i< k; i++ ) {
		item = ss.at(i);

		eRpn.term = item ;
        //  数値のチェック
        if( isdigit( item[0] ) ) {

			v = strtod( item.c_str(), &p ) ;

            if( *p != '\0' ) {
                throw eErrValue ;
            } else {
                if( v == HUGE_VAL || v == HUGE_VAL ) { //    HUGE_VAL
                    throw eErrOVFvalue ;
                }
            }
        } else if( !isOperators( item[0] ) ) {
        //  変数のチェック
        	mi = fTable->fMap.find( item ) ;
	    	if( mi == fTable->fMap.end() ) {    //  関数はスキップ
                if( !Calc.isSymbol( item ) ) {
//                    if( theIniFile != NULL && theIniFile->Opened ) {
//                        if( i != 0 )
//                            throw eErrUndefined ;
//                    } else {
                        Calc.setSymbol( item, 0.0 ) ;
//                    }
                }
		    }
        }

    }

    return( eErrOK ) ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : checkItem
//
// INPUT OUTPUT :
//		string		rpn					(I)	変換された式の文字列
//		int			(RETURN)			(R)	0:正常
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		前方参照のチェック
//----------------------------------------------------------------------------
int CalcRpn::checkRPN( string rpn )
{
	string	str ;
    unsigned int		i ;
    string	cell ;

    map< string, int >::iterator mi_F ;

//    if( !( theIniFile != NULL && theIniFile->Opened ) ) {
//	    return eErrOK ;
//    }


	unsigned int k = stringToken(rpn, " ");
	std::vector< string > ss( k ) ;
	for(unsigned int ii = 0; ii< k; ii++){
		ss.at(ii) = strTokenBuf[ii];
	}
    for( i = 0 ; i< k; i++ ) {
		str = ss.at(i);
        eRpn.term = str ;

        //  数値のチェック
        if( isdigit( str[0] ) ) {

        //  演算子のチェック
		}else if( isOperators( str[0] ) ) {
 
		}else{

        	mi_F = fTable->fMap.find( str ) ;
	    	if( mi_F != fTable->fMap.end() ) {    //  関数はスキップ

			}else{
				if( !Calc.isSymbol( str ) ) {
				   	throw eErrLvalue ;	//	Cann't happen!!
				}
			}
		}
	
    }

	return eErrOK ;
}

//----------------------------------------------------------------------------
// FUNCTION NAME : errMsg
//
// INPUT OUTPUT :
//		string		(RETURN)			(R)	エラーメッセージ
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		エラーメッセージの作成
//----------------------------------------------------------------------------
string  CalcRpn::errMsg( void )
{
    string  err ;
    char    temp[1024] ;

    sprintf( temp, "#%04d %s %s", eRpn.code, _errMsg( eRpn.code ), eRpn.term.c_str() ) ;

    return( err = temp ) ;
}
