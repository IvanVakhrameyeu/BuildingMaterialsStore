
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
----------ФИРМЫ-----------
--exec sp_rename 'Firms.CustPhoneNumber', 'FirmPhoneNumber', 'COLUMN'
 
create table Firms
(
FirmID int not null identity primary key,
FirmName varchar(300) not null, 
UNP varchar(200) not null,
FirmLegalAddress varchar(300) not null, 
FirmAccountNumber varchar(200) not null,
FirmBankDetails varchar(300) not null,
FirmDiscountAmount float not null,
FirmPhoneNumber varchar(30) null
)
/*------------НЕ АКТУАЛЬНО--------------
----------КЛИЕНТЫ----------- CTRL + K, CTRL + U (снять комменты)
--create table Customer
--(
--CustomerID int not null identity primary key,
--CustLastName varchar(20) not null, 
--CustFirstName varchar(20) not null, 
--CustPatronymic varchar(20) not null, 
--Sex varchar(2) not null ,
--CustDateOfBirth  Date not null, 
--CustAddress varchar(200) not null, 
--CustPhoneNumber varchar(20) not null, 
--CustDiscountAmount tinyint not null
)*/
--------------------------------------
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
UsersID int null foreign key references Users(UsersID),
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
----------СКЛАД-----------
create table Storage
(
StorageID  int not null identity primary key,
CategoryID int null foreign key references Category(CategoryID),
UnitID int null foreign key references Unit(UnitID),
[Name] varchar(100) not null,
[Count] tinyint not null,
[Description] varchar(350) not null,
Price float not null,
)

----------ПОКУПКИ-----------
create table Store
(
StoreID int not null identity primary key,
EmployeeID int null foreign key references Employee(EmployeeID),
FirmID int null foreign key references Firms(FirmID),
StorageID int null foreign key references Storage(StorageID),
[Count] tinyint not null,
TotalPrice float not null,
CurrentDiscountAmount float not null,
PurchaseDay Date not null,
Paid bit not null,
)

-------------------------ПРОЦЕДУРЫ
--SYSDATETIME()
--------------ДОБАВЛЕНИЕ ПОКУПКИ
--drop proc InputStore
go
create procedure InputStore 
@EmployeeID int,
@FirmID int,
@StorageID int,
@Count int,
@PurchaseDay Date
AS
begin
declare @Price float
declare @TotalPrice float
declare @CurrentDiscountAmount float 
set @CurrentDiscountAmount = (select FirmDiscountAmount from Firms where FirmID=@FirmID)
set @Price = (select Price from Storage where StorageID=@StorageID)
set @TotalPrice = ((@Price*@Count)-((@Price*@Count)*@CurrentDiscountAmount/100))

INSERT INTO Store(EmployeeID, FirmID, StorageID, [Count], TotalPrice, PurchaseDay, CurrentDiscountAmount,Paid)
                    VALUES (@EmployeeID, @FirmID, @StorageID, @Count, @TotalPrice, @PurchaseDay, @CurrentDiscountAmount,0)					
end
go
--exec InputStore @EmployeeID=3, @FirmID=3, @StorageID=3, @Count=3, @PurchaseDay='13.01.2019'
-------------------------------------------ДОБАВЛЕНИЕ КЛИЕНТА
--drop proc InsertFirm
go
create procedure InsertFirm 
@FirmName varchar(200), 
@UNP varchar(200), 
@FirmAccountNumber varchar(200), 
@FirmBankDetails varchar(200) ,
@FirmLegalAddress varchar(200),
@FirmPhoneNumber varchar(50)
AS
begin
INSERT INTO Firms(FirmName,UNP, FirmAccountNumber, FirmBankDetails, FirmLegalAddress, FirmPhoneNumber, FirmDiscountAmount)
        VALUES (@FirmName, @UNP, @FirmAccountNumber, @FirmBankDetails, @FirmLegalAddress, @FirmPhoneNumber, 0)					
end
go
--exec InsertFirm @StorageID=3, @Count=3
-------------------------------------------ДОБАВЛЕНИЕ НАИМЕНОВАНИЯ ТОВАРА
--drop proc InsertStoragesName
go
create procedure InsertStoragesName
@CategoryID int, 
@UnitID int, 
@Name varchar(100), 
@Description varchar(350),
@Price float
AS
begin
INSERT INTO Storage(CategoryID, UnitID, [Name], [Count], [Description],Price)
        VALUES (@CategoryID,@UnitID,@Name,0,@Description,@Price)					
end
go
--exec InsertStoragesName @CategoryID, @UnitID, @Name, @Description, @Price
-------------------------------------------ОТГРУЗКА ТОВАРА (изменить)
--drop proc Shipment
go
create procedure Shipment 
@ID int,
@Date Date
AS
begin
update Store 
set Paid=1
where FirmId=@ID and Store.PurchaseDay=@Date
end
go
--exec Shipment @ID=2
--update Store 
--set Paid=0
----------------- ------------------------
-------------------------------------------ОТГРУЗКА ТОВАРА (изменить)
--drop proc AddGoods
go
create procedure AddGoods 
@ID int,
@Count int
AS
begin
update Storage 
set [Count]=[Count]+@Count
where StorageID=@ID
end
go
----------------- ------------------------
insert Access(AccessName) 
values
('High'),
('Middle');
----------------------------------------
insert Users([Login], [Password],AccessID) 
values
('Admin',   '1',1),
('T',       '1',2),
('T3',      '1',2),
('T4',      '1',2),
('T5',      '1',2),
('T6',      '1',2),
('T7',      '1',2),
('T8',      '1',2),
('T9',      '1',2),
('T10',     '1',2),
('T11',     '1',2),
('T12',     '1',2),
('T13',     '1',2),
('T14',     '1',2),
('T15',     '1',2),
('T16',     '1',2),
('T1000',   '1',2);
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
insert Firms(FirmName,UNP, FirmAccountNumber, FirmBankDetails, FirmLegalAddress, FirmDiscountAmount) 
values 
('Государственное учреждение образования "повышения квалификации"', '190583856', 'BY40BPSB30121642290179330000', 'ЦБУ № 704 ОАО "БПС-Сбербанк", BIC: BPSBBY2X ЦБУ № 704 ОАО "БПС-Сбербанк"', 'г. Минск, 1-ый пер. Менделеева, д. 50, к. 4',0)	,
('Министерства природных ресурсов и охраны окружающей среды Республики Беларусь', '190583857', 'BY40BPSB30121642290179331111', 'ЦБУ № 704 ОАО "БПС-Сбербанк", BIC: BPSBBY2X ЦБУ № 705 ОАО "БПС-Сбербанк"', 'г. Минск, 1-ый пер. Менделеева, д. 50, к. 5',0)	; --,
--('', '', '', '', '',0)	;
------------------------
insert Unit (UnitName)
values
('л'),
('мм'),
('см'),
('м'),
('шт'), --5
('г'),
('кг'),
('уп');
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
(1, 5,'Гипсокартон и комплектующие',100,'Гипсокартон стеновой (12.5 мм) стандартный Knauf 60 x 150 см',4.20),
(1, 5,'Гипсокартон и комплектующие',100,'Нарезанный гипсокартон стеновой (12.5 мм) стандартный 60 x 120 см',3.09),
(1, 5,'Гипсокартон и комплектующие',100,'Гипсокартон стеновой (12.5 мм) стандартный Белгипс 120 x 250 см',9.49),
(1, 5,'Гипсокартон и комплектующие',100,'Гипсокартон стеновой (12.5 мм) влагостойкий Knauf 60 x 150 см',4.90),
(1, 5,'Гипсокартон и комплектующие',100,'Гипсокартон стеновой (12.5 мм) стандартный Белгипс 120 x 300 см',11.14),
(1, 5,'Смеси строительные',100,'Цемент (портландцемент) М500 Д20, 25 кг',5.49),
(1, 5,'Смеси строительные',100,'Цемент (портландцемент) М500 Д0, 25 кг',6.50),
(1, 5,'Смеси строительные',100,'Шпатлевка на полимерном связующем Weber Vetonit LR+, 20 кг',15.20),
(1, 5,'Смеси строительные',100,'Шпатлевка гипсовая Sniezka Acryl Putz ST10 (старт + финиш), 20 кг',15.98),
(1, 5,'Смеси строительные',100,'Шпатлевка гипсовая Knauf Fugen, 10 кг',7.60),
(1, 5,'Краски и эмали',100,'Краска акриловая Sniezka Ultra Biel белая, 1 л',4.12),
(1, 5,'Краски и эмали',100,'Краска акриловая Sniezka Ultra Biel белая, 3 л',11.33),
(1, 5,'Краски и эмали',100,'Краска акриловая Sniezka Ultra Biel белая, 5 л',17.30),
(1, 5,'Краски и эмали',100,'Краска акриловая Sniezka Ultra Biel белая, 10 л',29.32),
(1, 5,'Краски и эмали',100,'Краска акриловая Снежка Ультра Бель (РБ) белая, 5 л',16.85),
(1, 5,'Грунты и грунтовки',100,'Грунтовка желтая концентрат Ceresit CT17 Profigrunt, 2 л',7.39),
(1, 5,'Грунты и грунтовки',100,'Грунтовка желтая концентрат Ceresit CT17 Profigrunt, 5 л',17.68),
(1, 5,'Грунты и грунтовки',100,'Грунтовка желтая концентрат Ceresit CT17 Profigrunt, 10 л',28.80),
(1, 5,'Грунты и грунтовки',100,'Грунтовка бесцветная концентрат Ceresit CT17 Supergrunt, 2 л',7.75),
(1, 5,'Грунты и грунтовки',100,'Грунтовка бесцветная концентрат Ceresit CT17 Supergrunt, 5 л',18.38),
(1, 5,'Сетки, стеклохолст',100,'Сетка сварная ВР-1 (3.0 мм, 100 x 100 мм) 1 x 2 м',3.95),
(1, 5,'Сетки, стеклохолст',100,'Стеклохолст малярный "паутинка" Welton-эконом, 50 м2',25.79),
(1, 5,'Сетки, стеклохолст',100,'Стеклохолст малярный "паутинка" Nortex Deco, 50 м2',24.80),
(1, 5,'Сетки, стеклохолст',100,'Стеклохолст малярный "паутинка" Nortex Ultra, 50 м2',27.96),
(1, 5,'Сетки, стеклохолст',100,'Сетка стеклотканевая штукатурная ССАП-160, 10 м2',12.87),
(1, 5,'Уголки и маяки',100,'Уголок перфорированный алюминиевый, 2.5 м',0.98),
(1, 5,'Уголки и маяки',100,'Уголок перфорированный алюминиевый, 3 м',1.19),
(1, 5,'Уголки и маяки',100,'Уголок перфорированный стальной усиленный, 2.5 м',1.12),
(1, 5,'Уголки и маяки',100,'Уголок перфорированный стальной усиленный, 3 м',1.52),
(1, 5,'Уголки и маяки',100,'Уголок алюминиевый со стеклотканевой сеткой, 2.5 м',1.87),
(1, 5,'Средства для дерева',100,'Пропитка для дерева (антисептик) Goldbastik BB 20, 10 л (до 40 м.кв.), РБ',18.88),
(1, 5,'Средства для дерева',100,'Пропитка для дерева (антисептик) Goldbastik BB 20, 5 л (до 20 м.кв.), РБ',10.94),
(1, 5,'Средства для дерева',100,'Антисептик "Биоконтакт" Goldbastik BB26, усиленная защита древесины, 5 л',9.91),
(1, 5,'Средства для дерева',100,'Антисептик "Биоконтакт" Goldbastik BB26, усиленная защита древесины, 10 л',18.03),
(1, 5,'Средства для дерева',100,'Пропитка для дерева (антисептик) Vidaron концентрат 1:9 коричневая, 5 кг (80 м.кв.), РП',69.00),
(1, 5,'Клеи, герметики, пена',100,'Очиститель пены Эксперт, 500 мл',5.38),
(1, 5,'Клеи, герметики, пена',100,'Очиститель застывшей монтажной пены PENOSIL Cured Foam Remover 340 мл',9.65),
(1, 5,'Клеи, герметики, пена',100,'Очиститель для монтажной пены Penosil Foam Cleaner, 460 мл',9.65),
(1, 5,'Клеи, герметики, пена',100,'Клей для стеклообоев готовый Bostik 70, 5 л',24.67),
(1, 5,'Клеи, герметики, пена',100,'Клей для стеклообоев готовый Bostik 70, 15 л',53.31),
(2, 5,'Блоки и кирпичи',100,'Блок перегородочный (D500) 625 x 250 x 100 мм МКСИ',1.69),
(2, 5,'Блоки и кирпичи',100,'Блок перегородочный (D500) 625 x 250 x 120 мм МКСИ',2.09),
(2, 5,'Блоки и кирпичи',100,'Блок перегородочный (D500) 625 x 250 x 125 мм ЗАБУДОВА',2.38),
(2, 5,'Блоки и кирпичи',100,'Блок перегородочный (D500) 625 x 250 x 200 мм ЗАБУДОВА',3.60),
(2, 5,'Блоки и кирпичи',100,'Цемент (портландцемент) М500 Д20, 25 кг',5.49),
(3, 5,'Шифер',100,'Шифер хризотилцементный 8-волновой серый (5.2 мм) 113 x 175 см, РБ',7.20),
(3, 5,'Шифер',100,'Шифер хризотилцементный 8-волновой серый (5.8 мм) 113 x 175 см, РБ',8.20),
(3, 5,'Шифер',100,'Шифер хризотилцементный 8-волновой серый (5.2 мм) 113 x 175 см, РФ',7.80),
(3, 5,'Шифер',100,'Шифер хризотилцементный 8-волновой серый (5.8 мм) 113 x 175 см, РФ',8.40),
(3, 5,'Шифер',100,'Шифер хризотилцементный плоский серый (6 мм) 175 x 110 см, РБ',9.95),
(4, 5,'Отделка фасада',100,'Штукатурка ''шуба'' Тайфун Мастер №22С под окраску, 25 кг',12.90),
(4, 5,'Отделка фасада',100,'Штукатурка ''структурная'' Ceresit CT36 под окраску, 25 кг',12.89),
(4, 5,'Отделка фасада',100,'Краска фасадная акриловая Condor Fassandenfarde-Object, 15 кг',59.95),
(4, 5,'Отделка фасада',100,'Штукатурка ''короед'' (2.0 мм) Тайфун Мастер №23.2 под окраску, 25 кг',10.73),
(4, 5,'Отделка фасада',100,'Штукатурка ''короед'' (3.0 мм) Тайфун Мастер №23.3 под окраску, 25 кг',10.39),
(5, 5,'Кабели и провода',100,'Кабель ВВГ п-нг 2-х жильный медный, сечение 1,5мм (0,66 кВ)',1.06),
(5, 5,'Кабели и провода',100,'Кабель ВВГ п-нг 2-х жильный медный, сечение 2,5мм (0,66 кВ)',1.36),
(5, 5,'Кабели и провода',100,'Кабель ВВГ п-нг (А) 3-х жильный медный, сечение 2,5 мм (0,66 кВ)',1.88),
(5, 5,'Кабели и провода',100,'Провод ПВС 3х0,75мм медный (0,66 кВ) Автопровод, Щучин',0.65),
(5, 5,'Кабели и провода',100,'Провод ПВС-Т 2х0,75 белый, Автопровод-Щучин',0.59),
(6, 8,'Саморезы и шурупы',100,'Саморез 3.5*25 мм для монтажа ГКЛ к металлу, фосфат (100 шт в зип-локе) STARFIX (SMZ2-96507-100)',3.15),
(6, 8,'Саморезы и шурупы',100,'Саморез 3.5*35 мм для монтажа ГКЛ к металлу, фосфат (50 шт в зип-локе) STARFIX (SMZ2-96517-50)',2.38),
(6, 8,'Саморезы и шурупы',100,'Саморез 3.5*35 мм для монтажа ГКЛ к металлу, фосфат (1000 шт в карт. уп.) STARFIX (SMC3-96517-1000)',16.07),
(6, 8,'Саморезы и шурупы',100,'Саморез 3.5*45 мм для монтажа ГКЛ к металлу, фосфат (500 шт в карт. уп.) STARFIX (SMC3-96527-500)',9.72),
(6, 8,'Саморезы и шурупы',100,'Саморез 3.5*55 мм для монтажа ГКЛ к металлу, фосфат (500 шт в карт. уп.) STARFIX (SMC3-96537-500)',11.78),
(7, 5,'Кирпич для печи',100,'Кирпич керамический рядовой полнотелый одинарный КРО-200 КЕРАМИН',0.55),
(7, 5,'Кирпич для печи',100,'Кирпич рядовой полнотелый одинарный КРО М200 (Витебск, Цех №1)',0.60),
(7, 5,'Кирпич для печи',100,'Кирпич рядовой полнотелый одинарный КРО М200 с закругленным углом (Витебск, Цех №1)',0.95),
(7, 5,'Составы для печи',100,'Кладочно-штукатурная жаростойкая смесь Тайфун Мастер ПЕЧНИК, 25 кг',17.52),
(7, 5,'Составы для печи',100,'Порошок глиняный тарированный, 15кг',7.50),
(8, 5,'Садовый инвентарь',100,'Тачка садовая ECO WB6203-1 (65л, 150кг, 1 пневмоколесо 3.25-8) (WB6203-1)',53.84),
(8, 5,'Садовый инвентарь',100,'Тачка садовая DGM GT-1081 (80л, 150кг, 1 пневмоколесо 3.25-8) (GT-1081)',53.47),
(8, 5,'Садовый инвентарь',100,'Тачка строительно-садовая ТССР-1П (100л, 120 кг, 1 пневмоколесо 400*90мм, вес 15 кг) (КОМ) (ТССР-1П)',85.25),
(8, 5,'Садовый инвентарь',100,'Спанбонд №17 белый (2.1x10м) (4810751573830)',5.41),
(8, 5,'Садовый инвентарь',100,'Спанбонд №30 белый (рулон 1,6*300м, 960м.кв.) (1108568252004)',272.89);
insert Store (EmployeeID,FirmID,StorageID,[Count],TotalPrice,PurchaseDay, CurrentDiscountAmount,Paid)
values
(2,1,3,1,9.49,'12-1-2019',0,0),
(3,2,3,1,9.49,'12-1-2019',0,1),
(4,1,3,1,9.49,'12-1-2019',0,0),
(2,2,3,1,9.49,'12-1-2019',0,1),
(3,1,3,1,9.49,'12-1-2019',0,0);

---------------------------ТРИГЕРЫ
---------Триггер на подсчет TotalPrice (Store)
---------Триггер на проверку при покупки, что бы на складе было достаточно материалов


----------Тригер на изменение скидки покупатели после добавления покупки
go
create trigger insertStore
on Store
after insert
as
begin
---уменьшение товара на складе--------------------------------
declare @StorageID tinyint
declare @Count tinyint
declare @AvRating float
set @StorageID = (select inserted.StorageID from  inserted )
set @Count = (select inserted.[Count] from  inserted )
update Storage
set 
[Count]=[Count]-@Count
where StorageID=@StorageID
---------------------------------------------------------------
declare @TotalTotalPrice float
declare @TotalPrice float
declare @FirmID int
set @FirmID = (select inserted.FirmID from  inserted )
set @TotalTotalPrice = 0
set @TotalPrice = 0
declare cursorStore cursor  
--set @cursorStore = CURSOR scroll
for select TotalPrice from store where FirmID = @FirmID

open cursorStore
fetch next from cursorStore into @TotalPrice
while @@FETCH_STATUS=0
begin 
set @TotalTotalPrice = @TotalTotalPrice+@TotalPrice
fetch next from cursorStore into @TotalPrice
end
close cursorStore

if (@TotalTotalPrice <1000) 
begin 
update Firms
set FirmDiscountAmount = 1
where FirmID = @FirmID 
end 
if (@TotalTotalPrice <2000) 
begin 
update Firms
set FirmDiscountAmount = 2
where FirmID = @FirmID 
end 
if (@TotalTotalPrice <3000) 
begin 
update Firms
set FirmDiscountAmount = 3
where FirmID = @FirmID 
end 
--set @AvRating = (select Avg(Rating) from Logs where (Logs.StudentID = @ID))
--update Student set AverRating =  @AvRating where Student.StudentID=@ID
end
go

drop trigger insertStore


--select Storage.[Description], UnitName, Store.[Count], Store.TotalPrice, Storage.Price from Store
--join Storage on (Store.StorageID= Storage.StorageID)
--join Unit on (Storage.UnitID = Unit.UnitID)
--where Store.PurchaseDay='' and
--Store.FirmID=''
select FirmName, UNP, FirmLegalAddress, FirmAccountNumber,FirmBankDetails, FirmPhoneNumber from Firms where FirmID=2

select EmpFirstName, EmpLastName--, sum(Store.TotalPrice)  
                from Employee 
              --join Firms on(Store.FirmID = Firms.FirmID) 
                join Store on (Store.EmployeeID=Employee.EmployeeID)
              --join Storage on(Store.StorageID= Storage.StorageID) 
				group by EmpFirstName, 2

select sum(TotalPrice) from Store where EmployeeID = (select EmployeeID from Employee where EmpFirstName like '%4')

select Employee.EmpFirstName, Employee.EmpLastName, sum(TotalPrice) as "Продано на сумму"
from Store 
join Employee on (Store.EmployeeID=Employee.EmployeeID)
group by Employee.EmpFirstName, Employee.EmpLastName

		
		