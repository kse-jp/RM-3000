/* ================================================================	*//////////////////////////////////////////////////////////////////////
//
//  CalcMathErr.cpp
//  クラス
// 
//	2009-10-08	作成	中岡昌俊


#include "stdafx.h"

#include <math.h>
#include <string.h>
#include <stdio.h>
#include <string>

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

using namespace std ;

#include "CalcMathErr.h"


static char errstr[1024] ;

/* ----------------------------------------------------------------	*/
/*1:																*/
/*2:																*/
/*3:																*/
/*4:																*/
/*5:																*/
/*6:																*/
/* ----------------------------------------------------------------	*/
char    *_errMsg( int e )
{
    switch( e ) {
        case rErrSystem:    return "System Error" ;
        case iErrNoFile:    return "Not Found Ini-File" ;
        case iErrRead:      return "Ini-File Read Error" ;
        case iErrMeas:      return "Measuring-Field Error" ;
        case iErrTest:      return "Test-Files Error" ;
        case iErrExp:       return "Expression-Field Error" ;
        case mErrNoCh:      return "Not Found Measuring Channel" ;
        case mErrCh:        return "Measuring-Channel Error" ;
        case tErrNoCh:      return "Not Found Test-Channel" ;
        case tErrCh:        return "Test-Channel Error" ;
        case eErrNoCh:      return "Not Found Expression-Chanel" ;
        case eErrLvalue:    return "L-Value Error" ;
        case eErrAssinmentOperator: return "Not Found ""=""" ;
        case eErrNoExp:     return "No Expression" ;
        case eErrParenthesis:   return "Required '(' or ')'" ;
        case eErrFunction:  return "Function Format Error" ;
        case eErrRparenthesis:  return "Required ')'" ;
        case eErrLparenthesis:  return "Required '('" ;
        case eErrExtraParameter:return "Extra Parameter in function " ;
        case eErrTooFewParameter:return "Too Few Parameter in function " ;
        case eErrSyntax:    return "Expression Syntax Error" ;
        case eErrValue:     return "Numeric Value Error" ;
        case eErrOVFvalue:     return "Value OverFlow" ;
        case wErrForwardRef:    return "Warning-Forward Referencing" ;
        case eErrUndefined: return "Undeifned Symbol " ;
        default:            return "Undefined Error" ;
    }
}

/* ----------------------------------------------------------------	*/
/*1:																*/
/*2:																*/
/*3:																*/
/*4:																*/
/*5:																*/
/*6:																*/
/* ----------------------------------------------------------------	*/
int _matherrl( struct _exception *a)
{

	if ( !strcmp(a->name,"powl") || !strcmp(a->name,"sqrtl")
    		|| !strcmp(a->name,"fabsl") || !strcmp(a->name,"expl")
            || !strcmp(a->name,"log10l") || !strcmp(a->name,"logl")) {
		sprintf( errstr, "%s(%Lf,%Lf)", a->name, a->arg1, a->arg2 ) ;
    } else {
		sprintf( errstr, "MathError(%s)", a->name ) ;
    }
	throw RunError( errstr ) ;
}

/* ----------------------------------------------------------------	*/
/*1:																*/
/*2:																*/
/*3:																*/
/*4:																*/
/*5:																*/
/*6:																*/
/* ----------------------------------------------------------------	*/
int _matherr( struct _exception *a)
{

	if ( !strcmp(a->name,"powl") || !strcmp(a->name,"sqrtl")
    		|| !strcmp(a->name,"fabsl") || !strcmp(a->name,"expl")
            || !strcmp(a->name,"log10l") || !strcmp(a->name,"logl")) {
		sprintf( errstr, "%s(%f,%f)", a->name, a->arg1, a->arg2 ) ;
    } else {
		sprintf( errstr, "MathError(%s)", a->name ) ;
    }
	throw RunError( errstr ) ;
}
