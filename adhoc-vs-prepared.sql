-- Enrique Catalá Bañuls
--
-- Web:     http://www.solidq.com
-- Blog:    http://www.enriquecatala.com
-- Twitter: http://twitter.com/enriquecatala 
-- 
--
-- por qué es importante erradicar ad-hoc
--
------------------------------------------------------------------------------------------------------
-- configuración
--
------------------------------------------------------------------------------------------------------

use tempdb
go
set nocount on
go
if exists(select 1 from sys.tables where name ='detalle')	
	drop table dbo.detalle
go
if exists(select 1 from sys.tables where name ='maestro')	
	drop table dbo.maestro
go
IF OBJECT_ID('dbo.Seq1', 'SO') IS NOT NULL DROP SEQUENCE dbo.Seq1;
IF OBJECT_ID('dbo.Seq2', 'SO') IS NOT NULL DROP SEQUENCE dbo.Seq2;
GO
CREATE SEQUENCE dbo.Seq1 AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE dbo.Seq2 AS INT START WITH 1 INCREMENT BY 1;

create table dbo.maestro( 
  id_maestro int not null primary key default(NEXT VALUE FOR dbo.Seq1),
  data varchar(512) not null default ''
  )
go

-- detalle con clave ajena hacia maestro y su propio id gestionado con su propia secuencia
--
create table dbo.detalle( 
  id_detalle int not null primary key default(NEXT VALUE FOR dbo.Seq2),
  fk_maestro int not null constraint fk_detalle_maestro foreign key  references dbo.maestro(id_maestro),
  data varchar(512) not null default ''  
  )
go

------------------------------------------------------------------------------------------------------
-- Inserción de unas cuantas filas
--
------------------------------------------------------------------------------------------------------
-- Metemos unas pocas filas
insert into maestro(data) values ('hola'),('mundo')
go 2000

declare @i int=1;
while (@i<1998)
begin
	insert into detalle(fk_maestro,data) values (@i,'hola detalle'),(@i+1,'mundo detalle')
	set @i+=2
end
go

--select * from dbo.maestro m  inner join dbo.detalle d on m.id_maestro = d.fk_maestro
select * from dbo.maestro
select * from dbo.detalle
go
------------------------------------------------------------------------------------------------------
-- Ejecución parametrizada
-- 50k ejecuciones simulando aplicación corriendo
--   se lanza 50k veces la misma consulta cambiando el valor
-- MOSTRAR PERFORMANCE MONITOR
------------------------------------------------------------------------------------------------------
dbcc freeproccache --> nunca lanzar en producción esto!!!
go

declare @s1 nvarchar(max), @i2 int
declare @query nvarchar(1024), @queryExec nvarchar(1024)

set @query = N'declare @a int
              select @a = 1 
              from dbo.maestro m 
			      inner join dbo.detalle d on m.id_maestro = d.fk_maestro
		      where id_maestro=@p1'

declare @i int=1;
while (@i<50000)
begin	
	exec sp_executesql @stmt=@query,@params=N'@p1 int',@p1=@i
	set @i+=1
end
go

-- Podemos ver un único plan de eejecución (una única compilación)
-- utilizado 49999 veces
-- Atentos al tamaño de caché de procedimientos utilizada
-- 
SELECT usecounts, cacheobjtype, objtype, size_in_bytes, text 
FROM sys.dm_exec_cached_plans
CROSS APPLY sys.dm_exec_sql_text(plan_handle)
WHERE cacheobjtype LIKE 'Compiled Plan%' and text like '%from dbo.maestro%'
and text not like '%dm_exec_cached_plans%'



------------------------------------------------------------------------------------------------------
-- Ejecución AD-HOC
-- 50k ejecuciones simulando aplicación corriendo
--   se lanza 50k veces la misma consulta cambiando el valor
-- MOSTRAR PERFORMANCE MONITOR
------------------------------------------------------------------------------------------------------
-- Primero vaciamos la caché, para que no quede lugar a dudas
--
dbcc freeproccache

set nocount on;
declare @s1 nvarchar(max), @i2 int
declare @query nvarchar(1024), @queryExec nvarchar(1024)

set @query = N'declare @a int
              select @a = 1 
              from dbo.maestro m 
			      inner join dbo.detalle d on m.id_maestro = d.fk_maestro
		      where id_maestro='

declare @i int=1;
while (@i<50000)
begin
	set @queryExec = CONCAT(@query, @i)
	--print @queryExec
	exec(@queryExec)
	set @i+=1
end
go

-- Ahora vemos 49999 objetos en la caché de procedimientos
--
SELECT usecounts, cacheobjtype, objtype, size_in_bytes, text 
FROM sys.dm_exec_cached_plans
CROSS APPLY sys.dm_exec_sql_text(plan_handle)
WHERE cacheobjtype LIKE 'Compiled Plan%' and text like '%from dbo.maestro%'
and text not like '%dm_exec_cached_plans%'

-- Seamos un poco conscientes con números :)
-- ¿Que me ocupan mis escasas 50k consultitas?
--
SELECT sum(size_in_bytes) as size_in_bytes,
	   sum(size_in_bytes)/1024 as size_in_kb,
       sum(size_in_bytes)/1024/1024 as size_in_mb,
	   sum(size_in_bytes)*1.0/1024/1024/1024 as size_in_gigabytes
FROM sys.dm_exec_cached_plans
CROSS APPLY sys.dm_exec_sql_text(plan_handle)
WHERE cacheobjtype LIKE 'Compiled Plan%' and text like '%from dbo.maestro%'
and text not like '%dm_exec_cached_plans%'



------------------------------------------------------------------------------------------------------
-- Ejecución AD-HOC desde stored procedure
-- ojo, usar stored procedure o sp_executesql incorrectamente son igual de malos
--
------------------------------------------------------------------------------------------------------
-- Primero vaciamos la caché, para que no quede lugar a dudas
--
dbcc freeproccache
go

declare @s1 nvarchar(max), @i2 int
declare @query nvarchar(1024), @queryExec nvarchar(1024)

set @query = N'declare @a int
              select @a = 1 
              from dbo.maestro m 
			      inner join dbo.detalle d on m.id_maestro = d.fk_maestro
		      where id_maestro=1'
exec sp_executesql @query

set @query = N'declare @a int
              select @a = 1 
              from dbo.maestro m 
			      inner join dbo.detalle d on m.id_maestro = d.fk_maestro
		      where id_maestro=2'
exec sp_executesql @query

-- Como vemos, ya que se ha utilizado incorrectamente la parametrización, el plan de ejecución no se reutiliza
-- Lo mismo ocurriría en el caso de stored procedures que crean al vuelo una query...para eso mejor no hacerlos :)
--
SELECT usecounts, cacheobjtype, objtype, size_in_bytes, text 
FROM sys.dm_exec_cached_plans
CROSS APPLY sys.dm_exec_sql_text(plan_handle)
WHERE cacheobjtype LIKE 'Compiled Plan%' and text like '%from dbo.maestro%'
and text not like '%dm_exec_cached_plans%'
go


-- ENSEÑAR DEMO DE CURSORES!!!!!!