```sql
/*
create table users
(
	id serial primary key not null,
	name varchar(120) not null,
	age integer not null,	
	email varchar(120) not null unique,
	password varchar(1200) not null,
	role varchar(20) not null
)
*/
/*
create table posts
(
	id serial PRIMARY key not null,
	post_title varchar(250) not null,
	post_text varchar(8000) not null,
	publication_date TIMESTAMP not null,
	user_id int references users (id) on delete cascade
)
*/
/*
create table comments 
(
	id serial primary key not null,
	"commentText" varchar(5000) not null,
	"commentPublicationDate" Date not null,
	post_id INTEGER REFERENCES posts (id) on delete cascade,
	user_id integer REFERENCES users (id) on delete cascade
)
*/
/*
create table tegs
(
	id serial primary key not null,
	"tegTitle" varchar(100) not null
);
*/
/*
CREATE TABLE post_tegs (
post_id INTEGER REFERENCES posts (id) ON DELETE CASCADE,
teg_id INTEGER REFERENCES tegs (id) ON DELETE CASCADE,
PRIMARY KEY (post_id, teg_id)
);
*/
/*
create table userIDpostID
(
	"UserId" integer references users (id) ON DELETE CASCADE,
	"PostId" integer,
	foreign key ("PostId") references posts (id) ON DELETE cascade,
	primary key("UserId","PostId") /*это составной первичный ключ*/
)
*/
/*
ALTER TABLE Users
ALTER COLUMN Age int set not null
*/
/*
create table roles
(
	id serial primary key not null,
	role varchar(20) not null check (role IN ('admin', 'moder', 'user')),
	user_id integer references users (id) not null
)
*/
/*
alter table users 
add column role varchar(20) not null 
*/

```
