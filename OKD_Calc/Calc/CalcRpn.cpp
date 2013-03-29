/////////////////////////////////////////////////////////////////////
//
//  CalcRpn.cpp
//  ���Z���̉�̓N���X
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
//		1�̽�߰���ǉ�����
//		�ϐ����Q�����ȏ�ł���ʂł���悤��
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
	int		level_input;					// cp_input �̗D�揇��
	int		level_stack;					// cp_stack �̗D�揇��

	if(cp_stack == '\0' || cp_stack == '(') {
    	opStackPush( theOp ) ;
        putSpace() ;
        return ;
	}

    //�D�揇�ʂ�ݒ�
    if( theOp == '-' || theOp == '+' ) {	// �P���i�����j�̏���
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
//		���Z�q���ۂ��̃`�F�b�N
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
//		string		func				(I)	����������
//		int			start				(I)	�����J�n�ʒu
//		int			(RETURN)			(R)	')'�̈ʒu(���ޯ���l)
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�Ή�����')'�̌���
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
//		string &	strin				(I) �]�����鉉�Z���̕�����
//		int			(RETURN)			(R)	����
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�֐�����������
//		�֐���'('�̑O��'@'��}������Ă���
//		��r���Z�q��'.xx'�ɕϊ�
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


    //  �֐����̃`�F�b�N
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

    //  �p�����[�^���̃`�F�b�N
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
//		string &	strin				(I) ������
//		string &	strout				(O) ������
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
    	// �֐��ɂ� '@' ��}��
    	size = strin.size() ;
	    for( i = 0 ; i < size ; ) {
    	    c = strin[i++] ;
    		if(c == '(') {
            	c = strin[i-2] ;
            	if( ! isOperators( c ) ) {
			    	//'('�̎�O�̕��������Z�q�p�̗\�񕶎��łȂ��ꍇ�Ɋ֐��Ɣ��f����
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

        //  �������̑Ή�
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

        //  �֐��̃p�����[�^��
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
//		char *		input				(I) ������
//		char *		output				(O) ������
//		int 		size				(I) �ޯ̧����
//		int			(RETURN)			(R)	�����F0  ���s:1	
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		
//----------------------------------------------------------------------------
//	�X�^�b�N�ɂ�鐔���̂q�o�m�ւ̕ϊ��̍ۂ̃X�^�b�N����K��		
//	�i�֐����\���ł���悤�ɉ��ǁB�܂��A��]�␳�����ɂ��Ή��j      
//	L0:     �O�j�ꕶ���ǂݍ���                                      
//	L1:     �P�j�ϐ��A�萔�͂��̂܂܏o��                            
//	L2:     �Q�j���Z�q(op1)											
//	           �T�j�X�^�b�N����A�܂��̓X�^�b�N�̐擪��'('�̂Ƃ��A  
//					op1��push                                       
//	           �U�j�X�^�b�N�̐擪���A���Z�q(op2)�Ȃ�                
//	               op1��op2�̗D�揇�ʂ��ׂ�						
//	               ���j (op1�̗D�揇��) �� (op2�̗D�揇��)          
//	                      op2��pop                                  
//	               ���j (op1�̗D�揇��) �� (op2�̗D�揇��)          
//	                      op1��push                                 
//	L3:     �R�j'('��push                                           
//	L4:     �S�j')'�܂��́A������̏I���                           
//	           �T�j�X�^�b�N�̐擪��'('�̂Ƃ��A()��n��              
//	           �U�j�X�^�b�N�̐擪�����Z�q(op2)�Ȃ�                  
//	               ��)  op2��pop                                    
//	               ���j op2��'='�Ȃ�Apop���ďI��                   
//                                                                
//	"f(x)" �̂悤�Ȍ`������x�A"f@(x)" �̂悤�ɕϊ����āA			
//		'@'���֐��Ăяo���ł���Ɖ��肷��                           
//	op �̗D�揇��                                                   
//	  3 : '@'                                                       
//	  2 : '*', '/', '%' (�]��)                                      
//	  1 : '+', '-'                                                  
//	  0 : '='                                                       
//----------------------------------------------------------------------------
//	�g�p�����F
//		������Z�q(assignment operator)�������ꍇ�G���[
//		���Ӓl�ɉ��Z�q�A'@'�͎g�p�ł��Ȃ�
//----------------------------------------------------------------------------
int CalcRpn::strRPN(const char *input, char *output, int size )
{
	int		i ;
	int		c;
	string	strtemp( "" ) ;		// �ꎞ�I�Ɋi�[����
	string	strout( "" ) ;
	char	cp_input;			// *p_input ���i�[
	int		flag = 0;     		// '='�̗L��
    char    *p ;
	char	tmp[512];

	//�啶���ɕϊ�
	Tool_ChgUpperChar(input, tmp); 

    for( ; ; ) {
    	if( opStack.empty() )
        	break ;
        opStack.pop() ;
    }
	for( ; rpn.size() > 0 ; )
		rpn.erase() ;

// �G���[����    ���� '=' ���Ȃ��ƒv���I�ȃG���[�ɂȂ邩��
	i = 0;

	while((c = tmp[i]) != '\0') {
		if(c == '=') {
			flag = 1;
			break;
		}
//ADD 2009-11-17
		if( !flag && c == '.' ){		//"PHASE_DIST_SEL2.1="�Ή�
			goto NEXT_AAA;
		}
//ADD 2009-11-17
		if( ( isOperators( c ) == opCharFunc ) || ( isOperators( c ) == opCharLog )
        		|| (!flag && ( isOperators( c ) == opCharStd ) ) ) {

			//	���Ӓl�ɉ��Z�q�A'@'�͎g�p�ł��Ȃ�
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
	i = 0;       // �X�y�[�X������
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

	//	�������[�v
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

	//	�֐��̃p�����[�^�̋�؂�','���f'�ɒu��
	for( ; ; ) {
    	i = rpn.find( ',' ) ;
        if( i >= 0 )
        	rpn[i] = ' ' ;
        else
        	break ;
    }

	// �������ɑΉ������邽��
	// -d �� 0-d �Ƃ����Ӗ��ɂ���
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
//		string		rpn					(I)	�ϊ����ꂽ���̕�����
//		int			(RETURN)			(R)	0:����
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�ϊ����ꂽ���̕]��(��������)
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
        //  ���l�̃`�F�b�N
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
        //  �ϐ��̃`�F�b�N
        	mi = fTable->fMap.find( item ) ;
	    	if( mi == fTable->fMap.end() ) {    //  �֐��̓X�L�b�v
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
//		string		rpn					(I)	�ϊ����ꂽ���̕�����
//		int			(RETURN)			(R)	0:����
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�O���Q�Ƃ̃`�F�b�N
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

        //  ���l�̃`�F�b�N
        if( isdigit( str[0] ) ) {

        //  ���Z�q�̃`�F�b�N
		}else if( isOperators( str[0] ) ) {
 
		}else{

        	mi_F = fTable->fMap.find( str ) ;
	    	if( mi_F != fTable->fMap.end() ) {    //  �֐��̓X�L�b�v

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
//		string		(RETURN)			(R)	�G���[���b�Z�[�W
//----------------------------------------------------------------------------
// FUNCTION OF PROCEDURE
//		�G���[���b�Z�[�W�̍쐬
//----------------------------------------------------------------------------
string  CalcRpn::errMsg( void )
{
    string  err ;
    char    temp[1024] ;

    sprintf( temp, "#%04d %s %s", eRpn.code, _errMsg( eRpn.code ), eRpn.term.c_str() ) ;

    return( err = temp ) ;
}
