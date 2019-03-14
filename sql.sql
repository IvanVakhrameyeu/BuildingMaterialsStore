
use master 
create database storedb
use storedb
---------Дропнуть---------
drop database storedb
------------1------------

-----------ЕД ИЗМЕРЕНИЯ-------------
create table Unit
(
UnitID int not null identity primary key,
UnitName varchar(5) not null
)
----------ДОСТУП-----------------
create table Access
(
AccessID int not null identity primary key,
AccessName varchar(10) not null
)
----------КЛИЕНТЫ-----------
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
----------ПОЛЬЗОВАТЕЛИ-----------
create table Users
(
UsersID int not null identity primary key,
AccessID int foreign key references Access(AccessID),
[Login] varchar(50) not null,
[Password] varchar(50) not null,
);
--------------РАБОТНИК--------------
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
--constrain CheckTeacher check (Sex = 'М' OR Sex= 'Ж') 
);
----------КАТЕГОРИИ ТОВАРА-------
create table Category
(
CategoryID  int not null identity primary key,
NameCategory varchar(20) not null,
)

select Storage.StorageID, Category.NameCategory, Unit.UnitName, Storage.[Name], 
Storage.[Count], Storage.[Description], Storage.Price from Storage
                        join Unit on (Storage.UnitID=Unit.UnitID)
						join Category on (Storage.CategoryID=Category.CategoryID)
----------СКЛАД-----------
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
----------МАГАЗИН-----------
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
('1','Степанов','Admin','Викторович',   'М', '1982-08-27','г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('2','Баварец','Оля',  'Анатольевич',  'Ж', '1982-10-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('3','Баварец','Степан', 'Анатольевич',  'М', '1982-02-27','г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('4','Баварец','Женя',   'Анатольевич', 'Ж', '1982-01-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('5','Баварец','Антон',   'Анатольевич', 'М', '1982-03-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('6','Баварец','Виктория', 'Анатольевич',   'Ж', '1982-04-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('7','Баварец','Григорий',  'Анатольевич',  'М', '1982-05-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('8','Баварец','Ваня', 'Анатольевич',   'М', '1982-06-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('9','Баварец','Дима',  'Анатольевич',  'М', '1982-07-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('10','Баварец','Паша',  'Анатольевич',  'М', '1982-01-18' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('11','Баварец','Анатолий',  'Анатольевич',  'М', '1982-01-19' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('12','Баварец','Егор',   'Анатольевич', 'М', '1982-01-20' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('13','Баварец','Никитий','Анатольевич',    'М', '1982-01-21' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('14','Баварец','Артем','Анатольевич','М', '1982-01-22' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('15','Баварец','Леха','Анатольевич','М', '1982-01-23' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('16','Баварец','Данииил','Анатольевич',    'М', '1982-01-24' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3),
('17','Баварец','Саша', 'Анатольевич',   'Ж', '1982-01-25' ,'г.Минск ул Слободская д58 кв 33', '152-33-32', 'Администратор',3);

insert Customer(CustLastName, CustFirstName, CustPatronymic, Sex, CustDateOfBirth, CustAddress, CustPhoneNumber) 
values 
('Степанов','Admin','Викторович',   'М', '1982-08-27','г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Вася', 'Викторович',   'М', '1982-09-27','г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Оля',  'Викторович',  'Ж', '1982-10-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Степан', 'Викторович',  'М', '1982-02-27','г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Женя',   'Викторович', 'Ж', '1982-01-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Антон',   'Викторович', 'М', '1982-03-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Виктория', 'Викторович',   'Ж', '1982-04-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Григорий',  'Викторович',  'М', '1982-05-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Ваня', 'Викторович',   'М', '1982-06-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Дима',  'Викторович',  'М', '1982-07-17' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Паша',  'Викторович',  'М', '1982-01-18' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Анатолий',  'Викторович',  'М', '1982-01-19' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Егор',   'Викторович', 'М', '1982-01-20' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Никитий','Викторович',    'М', '1982-01-21' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Артем','Викторович','М', '1982-01-22' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Леха','Викторович','М', '1982-01-23' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Данииил','Викторович',    'М', '1982-01-24' ,'г.Минск ул Слободская д58 кв 33', '152-33-32'),
('Антонов','Саша', 'Викторович',   'Ж', '1982-01-25' ,'г.Минск ул Слободская д58 кв 33', '152-33-32');
------------------------
insert Unit (UnitName)
values
('л'),
('мм'),
('см'),
('м'),
('шт'),
('г'),
('кг');

-------------------
insert Category (NameCategory)
values 
('Отделочные материалы'),
('Общестроительные'),
('Кровельные материалы'),
('Фасадные материалы'),
('Электрика'),
('Метизы и крепеж'),
('Печные материалы'),
('Сад и огород');
-----------------------------------
insert Storage (CategoryID, UnitID, [Name], [Count], [Description], Price)
values
(1, 5,'Гипсокартон',100,'Гипсокартон стеновой (12.5 мм) стандартный Knauf 60 x 150 см',4.20),
(1, 5,'Гипсокартон',100,'Нарезанный гипсокартон стеновой (12.5 мм) стандартный 60 x 120 см',3.09),
(1, 5,'Гипсокартон',100,'Гипсокартон стеновой (12.5 мм) стандартный Белгипс 120 x 250 см',9.49);
---------------------------ТРИГЕРЫ
insert Store (EmployeeID,CustomerID,[Count],TotalPrice)
values
(2,1,3,12.8),
(3,2,3,12.8),
(4,4,3,12.8),
(2,3,3,12.8),
(3,2,3,12.8);


---------Триггер на подсчет TotalPrice (Store)
---------Триггер на проверку при покупки, что бы на складе было достаточно материалов