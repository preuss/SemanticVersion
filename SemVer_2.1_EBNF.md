## Special Semantic Version v2.1
This is the Extended Barckus Naur Form for the Special Semantic Version v2.1.
It is compatible with SemVer 2.0 
### EBNF - Extended Barckus–Naur Form

```

<valid semver> ::= <version core>
                 | <version core> "-" <release>
                 | <version core> "+" <build>
                 | <version core> "-" <release> "+" <build>

<version core> ::= <major> ( <dot> <minor> ( <dot> <patch> )? )?

<major> ::= <numeric identifier>

<minor> ::= <numeric identifier>

<patch> ::= <numeric identifier>

<release> ::= <release identifier> ( <dot> <release identifier> )* ;

<build> ::= <build identifier> ( <dot> <build identifier> )* ;

<release identifier> ::= <string> ( <dot> <release stage> )? ;

<build identifier> ::= <string> ( <dot> <build count> )? ;

<numeric identifier> ::= <number> ;

<identifier> ::= <string> ;

<string> ::= <special alphanumeric>
           | <string> <special alphanumeric> ;

<special alphanumeric> ::= <alphanumeric>
                         | <special character> ;

<alphanumeric> ::= <letter> 
                 | <number> ;

<special character> ::= "-" 
                      | "_" | <dot>
                      | "$" | "£" | "€" | "¤" | "#" | "@" 
                      | "'" | "!" | "^" | "~" | "|" | ";"
                      | "{" | "}" | "[" | "]" | "(" | ")" 
                      ;

<release stage> ::= <number> ;

<build count> ::= <number> ;

<release prefix> ::= "-" ;

<build prefix> ::= "+" ;

<dot> ::= "." ;

<number> ::= <digit>
           | <number> <digit> ;

<digit> ::= "0".."9" ;

<letter> ::= <ascii letter> 
           | <danish extra letter> ;

<danish extra letter> ::=  "æ" | "ø" | "å" 
                         | "Æ" | "Ø" | "Å" ;

<ascii letter> ::= "a".."z" 
                 | "A".."Z" ;

```