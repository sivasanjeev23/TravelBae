
-- Available buses table
create table TbBuses(bid int primary key identity(100,1),TravelsName varchar(20) not null,
Boarding varchar(20) not null,Dropping varchar(20) not null,Remseat int,
[Type] varchar(10) check ([Type] in ('AC','Non-AC')),Totseat int)

-- Values to be inserted into bus table
insert into TbBuses values
('Lakshmi','Madurai','Salem',30,'AC',30),
('Banu','Madurai','Salem',50,'Non-AC',50),
('Suba','Madurai','Salem',50,'Non-AC',50),
('Ashwin','Chennai','Coimbatore',50,'Non-AC',50),
('SRS','Chennai','Coimbatore',50,'AC',50),
('DMS','Chennai','Coimbatore',50,'AC',50),
('Ramu','Madurai','Coimbatore',30,'AC',30),
('DMS','Chennai','Salem',30,'AC',30),
('DMS','Chennai','Coimbatore',30,'AC',30)

-- Table for storing users profile
create table Tbuser([Uid] int primary key identity(2000,1),
[Name] varchar(20),
email varchar(50) check(email like('%_@_%.com')),
Mblno bigint,
Gender char check(Gender in ('M','F')),
Dob date,
city varchar(20))

-- Table to get Booking
create table booking(Bid int foreign key references tbbuses(bid),Pname varchar(20),
[Uid] int foreign key references tbuser([Uid]),Bookid int primary key identity(20000,1))

-- Stored Procedure need to be created
create proc sp_pickuseMbl @mbl bigint
as begin 
select * from Tbuser where Mblno=@mbl
end

create proc sp_pickuseEm @em varchar(30)
as begin 
select * from Tbuser where email=@em
end 

create proc sp_insMbl @mbl Bigint
as begin 
insert into tbuser(mblno) values(@mbl)
end  

create proc sp_insEm @em varchar(30)
as begin 
insert into tbuser(email) values(@em)
end 

create proc sp_pinsMbl @mbl Bigint,@em varchar(30)
as begin 
update tbuser set mblno=@mbl where email=@em
end  

create proc sp_pinsEm @em varchar(30),@mbl Bigint
as begin 
update tbuser set email=@em where mblno=@mbl
end  

create proc Sp_upPro @nam varchar(20),@gen char(1),@dob date,@city varchar(20),@mbl bigint,
@em varchar(30)
as begin
update tbuser set name=@nam,gender=@gen,dob=@dob,city=@city 
where mblno=@mbl or email =@em
end

create proc sp_view @mblno bigint
as
begin
select b.[pname]as [Passenger Name],u.mblno as MobileNo,t.bid as BusNo,
t.travelsname as BusName,
t.Boarding,t.Dropping,b.Bookid as Bookingid from tbbuses t join
booking b on b.bid=t.bid join tbuser u on u.[Uid]=b.[Uid] where u.Mblno=@mblno
end

create proc sp_board
as begin 
Select distinct boarding from tbbuses
end  

create proc sp_drop
as begin 
Select distinct dropping from tbbuses
end

create proc sp_viewbus @board varchar(20),@drop varchar(20)
as begin 
select bid,travelsname,boarding,dropping,[type],Remseat from tbbuses 
where Boarding=@board and Dropping=@drop
end

create proc sp_checkseat @bid int
as begin 
select remseat from tbbuses where Bid=@bid
end

create proc sp_picUser @em varchar(30)
as begin 
Select [uid],[name] from tbuser where email=@em
end

create proc sp_insbook @bid int,@nam varchar(20),@uid int
as begin 
insert into booking(bid,pname,[uid]) values(@bid,@nam,@uid)
end

create proc sp_setRemseat @bid int
as begin 
update TbBuses set RemSeat-=1 where bid=@bid
end

-- To view Available Tables
select * from TbBuses
select * from Tbuser
select * from booking

