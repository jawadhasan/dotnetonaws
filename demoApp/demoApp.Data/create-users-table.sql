CREATE TABLE users (
  id SERIAL PRIMARY KEY NOT NULL, 
  email VARCHAR(255) UNIQUE NOT NULL,
  firstName VARCHAR(50),
  lastName VARCHAR(50),
  createdat TIMESTAMPTZ DEFAULT NOW()
  );

 CREATE TABLE userposts (
  id SERIAL PRIMARY KEY NOT NULL, 
  userid int,
  FOREIGN KEY (userid) REFERENCES users(id) ON DELETE CASCADE,

  title VARCHAR(255) UNIQUE NOT NULL,
  content VARCHAR(255), 
  createdat TIMESTAMPTZ DEFAULT NOW()
  );


  
INSERT INTO userposts(id,userid, title,content,createdat)
VALUES
(1,1,'A Brief Time of History','Content about brief history of time.',now()),
(2,1,'A Tale of Two Cities','Content about tale of two cities.',now()),
(3,2,'When OS spies on You','content when OS spies on you.',now()),
(4,2,'ApplnoidOS Beta','content apple and android jv',now()),
(5,3,'A kings tale','content about kings tale.',now())