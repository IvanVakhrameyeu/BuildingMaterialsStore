
use master 
create database storedb
use storedb
---------��������---------
drop database storedb
------------1------------

-----------�� ���������-------------
create table Unit
(
UnitID int not null identity primary key,
UnitName varchar(5) not null
)
----------������-----------------
create table Access
(
AccessID int not null identity primary key,
AccessName varchar(10) not null
)
----------�������-----------
create table Customer
(
CustomerID int not null identity primary key,
CustLastName varchar(20) not null, 
CustFirstName varchar(20) not null, 
CustPatronymic varchar(20) not null, 
Sex varchar(2) not null ,
CustDateOfBirth  Date not null, 
CustAddress varchar(200) not null, 
CustPhoneNumber varchar(20) not null, 
)
----------������������-----------
create table Users
(
UsersID int not null identity primary key,
AccessID int foreign key references Access(AccessID),
[Login] varchar(50) not null,
[Password] varchar(50) not null,
);
--------------��������--------------
create table Employee 
(EmployeeID int not null identity primary key, 
UsersID int foreign key references Users(UsersID),
EmpLastName varchar(20) not null, 
EmpFirstName varchar(20) not null, 
EmpPatronymic varchar(20) not null, 
Sex varchar(2) not null ,
EmpDateOfBirth Date not null, 
EmpAddress varchar(200) not null,
EmpPhoneNumber varchar(20) not null,
Position varchar(20) not null,
Experience tinyint not null,
--constrain CheckTeacher check (Sex = '�' OR Sex= '�') 
);
----------��������� ������-------
create table Category
(
CategoryID  int not null identity primary key,
NameCategory varchar(20) not null,
)

select Storage.StorageID, Category.NameCategory, Unit.UnitName, Storage.[Name], 
Storage.[Count], Storage.[Description], Storage.Price from Storage
                        join Unit on (Storage.UnitID=Unit.UnitID)
						join Category on (Storage.CategoryID=Category.CategoryID)
----------�����-----------
create table Storage
(
StorageID  int not null identity primary key,
CategoryID int null foreign key references Category(CategoryID),
UnitID int null foreign key references Unit(UnitID),
[Name] varchar(20) not null,
[Count] tinyint not null,
[Description] varchar(100) not null,
Price float not null,
)
----------�������-----------
create table Store
(
StorageID int not null identity primary key,
EmployeeID int null foreign key references Employee(EmployeeID),
CustomerID int null foreign key references Customer(CustomerID),
[Count] tinyint not null,
TotalPrice float not null,
)
----------------- ------------------------
insert Access(AccessName) 
values
('High'),
('Middle');
----------------------------------------
insert Users([Login], [Password],AccessID) 
values
('Admin',   '1',1),
('Teacher',    '1',2),
('Teacher3',    '1',2),
('Teacher4',    '1',2),
('Teacher5',    '1',2),
('Teacher6',    '1',2),
('Teacher7',    '1',2),
('Teacher8',    '1',2),
('Teacher9',    '1',2),
('Teacher10',    '1',2),
('Teacher11',    '1',2),
('Teacher12',    '1',2),
('Teacher13',    '1',2),
('Teacher14',    '1',2),
('Teacher15',    '1',2),
('Teacher16',    '1',2),
('Teacher17',    '1',2);
--------------------------------------------------
insert Employee(UsersID,EmpLastName, EmpFirstName, EmpPatronymic, Sex, EmpDateOfBirth, EmpAddress, EmpPhoneNumber, Position, Experience) 
values 
('1','��������','Admin','����������',   '�', '1982-08-27','�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('2','�������','���',  '�����������',  '�', '1982-10-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('3','�������','������', '�����������',  '�', '1982-02-27','�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('4','�������','����',   '�����������', '�', '1982-01-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('5','�������','�����',   '�����������', '�', '1982-03-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('6','�������','��������', '�����������',   '�', '1982-04-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('7','�������','��������',  '�����������',  '�', '1982-05-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('8','�������','����', '�����������',   '�', '1982-06-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('9','�������','����',  '�����������',  '�', '1982-07-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('10','�������','����',  '�����������',  '�', '1982-01-18' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('11','�������','��������',  '�����������',  '�', '1982-01-19' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('12','�������','����',   '�����������', '�', '1982-01-20' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('13','�������','�������','�����������',    '�', '1982-01-21' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('14','�������','�����','�����������','�', '1982-01-22' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('15','�������','����','�����������','�', '1982-01-23' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('16','�������','�������','�����������',    '�', '1982-01-24' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3),
('17','�������','����', '�����������',   '�', '1982-01-25' ,'�.����� �� ���������� �58 �� 33', '152-33-32', '�������������',3);

insert Customer(CustLastName, CustFirstName, CustPatronymic, Sex, CustDateOfBirth, CustAddress, CustPhoneNumber) 
values 
('��������','Admin','����������',   '�', '1982-08-27','�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','����', '����������',   '�', '1982-09-27','�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','���',  '����������',  '�', '1982-10-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','������', '����������',  '�', '1982-02-27','�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','����',   '����������', '�', '1982-01-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','�����',   '����������', '�', '1982-03-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','��������', '����������',   '�', '1982-04-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','��������',  '����������',  '�', '1982-05-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','����', '����������',   '�', '1982-06-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','����',  '����������',  '�', '1982-07-17' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','����',  '����������',  '�', '1982-01-18' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','��������',  '����������',  '�', '1982-01-19' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','����',   '����������', '�', '1982-01-20' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','�������','����������',    '�', '1982-01-21' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','�����','����������','�', '1982-01-22' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','����','����������','�', '1982-01-23' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','�������','����������',    '�', '1982-01-24' ,'�.����� �� ���������� �58 �� 33', '152-33-32'),
('�������','����', '����������',   '�', '1982-01-25' ,'�.����� �� ���������� �58 �� 33', '152-33-32');
------------------------
insert Unit (UnitName)
values
('�'),
('��'),
('��'),
('�'),
('��'),
('�'),
('��');

-------------------
insert Category (NameCategory)
values 
('���������� ���������'),
('����������������'),
('���������� ���������'),
('�������� ���������'),
('���������'),
('������ � ������'),
('������ ���������'),
('��� � ������');
-----------------------------------
insert Storage (CategoryID, UnitID, [Name], [Count], [Description], Price)
values
(1, 5,'�����������',100,'����������� �������� (12.5 ��) ����������� Knauf 60 x 150 ��',4.20),
(1, 5,'�����������',100,'���������� ����������� �������� (12.5 ��) ����������� 60 x 120 ��',3.09),
(1, 5,'�����������',100,'����������� �������� (12.5 ��) ����������� ������� 120 x 250 ��',9.49);
---------------------------�������
insert Store (EmployeeID,CustomerID,[Count],TotalPrice)
values
(2,1,3,12.8),
(3,2,3,12.8),
(4,4,3,12.8),
(2,3,3,12.8),
(3,2,3,12.8);


---------������� �� ������� TotalPrice (Store)
---------������� �� �������� ��� �������, ��� �� �� ������ ���� ���������� ����������