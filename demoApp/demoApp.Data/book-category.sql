--create a book table and bookcategory table 
--where a category can have many books, 
--but a book can belong to only one category.

create table bookcategory (
    id serial primary key unique,
    categoryname varchar(255)
);

create table book (
    id serial primary key unique ,
    bookcategoryid serial references bookcategory(id),
    bookname varchar(255) not null,
    price int not null,
    createdat TIMESTAMPTZ not null
)


INSERT INTO bookcategory(id, categoryname)
VALUES
(10,'History'),
(20,'Ficktion'),
(30,'Kids'),
(40,'IT'),
(50,'Darama'),
(100,'NewArrival')

INSERT INTO book(id,bookcategoryid, bookname,price,createdat)
VALUES
(1,10,'A Brief Time of History',100,now()),
(2,100,'A Tale of Two Cities',50,now()),
(3,100,'When OS spies on You',20,now()),
(4,100,'ApplnoidOS Beta',20,now()),
(5,30,'A kings tale',60,now())
