
ALTER    FUNCTION [dbo].[fn_Split]
    (
      @text NVARCHAR(MAX) ,
      @separator VARCHAR(20) = ','
    )
RETURNS @strings TABLE
    (
      [position] INT IDENTITY PRIMARY KEY ,
      [value] NVARCHAR(MAX)
    )
AS
    BEGIN
        DECLARE @index INT;
        SET @index = -1;
        WHILE ( LEN(@text) > 0 )
            BEGIN
                SET @index = CHARINDEX(@separator, @text);
                IF ( @index = 0 )
                    AND ( LEN(@text) > 0 )
                    BEGIN
                        INSERT  INTO @strings
                        VALUES  ( @text );
                        BREAK;
                    END;
                IF ( @index > 1 )
                    BEGIN
                        INSERT  INTO @strings
                        VALUES  ( LEFT(@text, @index - 1) );
                        SET @text = RIGHT(@text, ( LEN(@text) - @index ));
                    END;
                ELSE
                    SET @text = RIGHT(@text, ( LEN(@text) - @index ));
            END;
        RETURN;
    END;