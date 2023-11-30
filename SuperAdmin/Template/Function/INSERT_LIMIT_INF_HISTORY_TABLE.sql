CREATE OR REPLACE FUNCTION public."INSERT_LIMIT_INF_HISTORY_TABLE"()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$DECLARE
    REVISION_NUMBER bigint;
    HISTORY_TABLE_NAME character varying(63);
    clientAddr varchar(100);
    clientHostName varchar(100);
	hokenPid int;
BEGIN
--
-- Create a row in history table to reflect the operation performed on original one,
-- make use of the special variable TG_OP to work out the operation.
--
hokenPid = NEW."HOKEN_PID";
if (hokenPid != 0) then return null;
end if;

hokenPid = OLD."HOKEN_PID";
if (hokenPid != 0) then return null;
end if;

HISTORY_TABLE_NAME := 'Z_' || TG_TABLE_NAME;
execute format('SELECT COALESCE(MAX("OP_ID"), 0) FROM %I ', HISTORY_TABLE_NAME) into REVISION_NUMBER;
REVISION_NUMBER := REVISION_NUMBER + 1;
select CLIENT_ADDR, CLIENT_HOSTNAME from pg_stat_activity where pid = pg_backend_pid() into clientAddr, clientHostName;

if (TG_OP = 'INSERT') then
    execute format('INSERT INTO %I '
                   'SELECT $1, $2, $3, $4, $5, $6.*', HISTORY_TABLE_NAME)
                   using REVISION_NUMBER, 'INSERT', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'UPDATE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.*', HISTORY_TABLE_NAME)
                    using REVISION_NUMBER, 'UPDATE', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'DELETE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.*', HISTORY_TABLE_NAME)
                    using REVISION_NUMBER, 'DELETE', current_timestamp, clientAddr, clientHostName, old;
    return old;
end if;
return null; -- result is ignored since this is an AFTER trigger
end;$function$
;
