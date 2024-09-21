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

create table author (
    id serial primary key unique,
    authorname varchar(255)
);

CREATE TABLE bookauthor (
    id serial primary key unique,
    bookid INT,
    authorid INT,   
    FOREIGN KEY (bookid) REFERENCES book(id) ON DELETE CASCADE,
    FOREIGN KEY (authorid) REFERENCES author(id) ON DELETE CASCADE
);

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


INSERT INTO author(id, authorname)
VALUES
(10,'Jane Green'),
(20,'stephen hawking'),
(30,'JK Rowling'),
(40,'Test Author1'),
(50,'Test Author2')




INSERT INTO bookauthor(id, bookid, authorid)
VALUES
(1,1,20),
(2,2,40),
(3,2,50),
(3,3,40),
(4,3,40),
(5,4,40),
(6,5,50)


