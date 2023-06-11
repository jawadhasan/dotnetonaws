CREATE TABLE notes (
    id serial NOT NULL PRIMARY KEY,
    title varchar(255) NOT NULL,
    content varchar(255) NOT NULL,
    details varchar,
    createdat TIMESTAMPTZ DEFAULT NOW(),
    categoryid INTEGER DEFAULT 1 NOT NULL,
    userid varchar(255) DEFAULT 'notesappuser' NOT NULL
)