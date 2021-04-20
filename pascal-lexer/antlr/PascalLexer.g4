lexer grammar PascalLexer;

// Section: delimiters
WS: [ \t\r\n] -> channel(HIDDEN);


// Section: numbers
UnsignedNumber: UnsignedInteger | UnsignedReal;
SignedNumber: Sign? UnsignedNumber;
fragment UnsignedInteger
    : DigitSeq 
    | Dollar HexDigitSeq
    | Ampersand OctalDigitSeq
    | Percend BinDigitSeq; 
fragment UnsignedReal: DigitSeq ('.' DigitSeq)? ScaleFactor?;
fragment DigitSeq: Digit+;
fragment BinDigitSeq: BinDigit+;
fragment HexDigitSeq: HexDigit+;
fragment OctalDigitSeq: OctalDigit+;

fragment HexDigit: [0-9a-fA-F];
fragment OctalDigit: '0' .. '7';
fragment BinDigit: '0' | '1';
fragment Digit: '0' .. '9';
fragment Sign: PLUS | MINUS;
fragment ScaleFactor: ('E' | 'e') Sign? DigitSeq;  


// Section: comments
SINGLE_COMMENT: '//' ~[\r\n]* -> channel(HIDDEN);
MultiComment1: LPAREN STAR (DelimitedComment | . )*? STAR RPAREN -> channel(HIDDEN);
MultiComment2: LCURLY  (DelimitedComment | . )*? RCURLY -> channel(HIDDEN);
fragment DelimitedComment
    : MultiComment1
    | MultiComment2
    ;


// Section: identifiers
IDENT: Letter (Letter | Digit | Underscore)*;

fragment Underscore: '_';
fragment Dollar: '$';
fragment Ampersand: '&';
fragment Percend: '%';
fragment Letter: [a-zA-Z];


SYMBOL
    : Letter
    | Digit
    | HexDigit
    ;


BAD_CHARACTER: .;


fragment PLUS: '+';
fragment MINUS: '-';
fragment STAR: '*';
fragment LPAREN: '(';
fragment RPAREN: ')';
fragment LCURLY: '{';
fragment RCURLY: '}';
