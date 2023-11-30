CREATE OR REPLACE FUNCTION public."INSERT_HISTORY_TABLE"()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$DECLARE
    HISTORY_TABLE_NAME character varying(63);
    clientAddr varchar(100);
    clientHostName varchar(100);
BEGIN
--
-- Create a row in history table to reflect the operation performed on original one,
-- make use of the special variable TG_OP to work out the operation.
--
HISTORY_TABLE_NAME := 'Z_' || TG_TABLE_NAME;
select CLIENT_ADDR, CLIENT_HOSTNAME from pg_stat_activity where pid = pg_backend_pid() into clientAddr, clientHostName;

if (TG_OP = 'INSERT') then
    execute format('INSERT INTO %I '
                   'SELECT $1, $2, $3, $4, $5, $6.* ON CONFLICT ("OP_ID") DO NOTHING', HISTORY_TABLE_NAME)
                   using nextval('"OP_ID_seq"'::regclass), 'INSERT', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'UPDATE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.* ON CONFLICT ("OP_ID") DO NOTHING', HISTORY_TABLE_NAME)
                    using nextval('"OP_ID_seq"'::regclass), 'UPDATE', current_timestamp, clientAddr, clientHostName, new;
    return new;
elsif (TG_OP = 'DELETE') then
    execute format('INSERT INTO %I '  
                   'SELECT $1, $2, $3, $4, $5, $6.* ON CONFLICT ("OP_ID") DO NOTHING', HISTORY_TABLE_NAME)
                    using nextval('"OP_ID_seq"'::regclass), 'DELETE', current_timestamp, clientAddr, clientHostName, old;
    return old;
end if;
return null; -- result is ignored since this is an AFTER trigger
end;$function$
;
