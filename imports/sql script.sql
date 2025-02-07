BEGIN;

CREATE TABLE IF NOT EXISTS public.events
(
    event_id serial PRIMARY KEY,
    event_name character varying(255) COLLATE pg_catalog."default" NOT NULL,
    event_type character varying(255) COLLATE pg_catalog."default" NOT NULL,
    status character varying(255) COLLATE pg_catalog."default" NOT NULL,
    date timestamp without time zone,
    description text COLLATE pg_catalog."default"
);

CREATE TABLE IF NOT EXISTS public.subdepartments
(
    subdepartment_id serial PRIMARY KEY,
    subdepartment_name character varying(255) COLLATE pg_catalog."default" NOT NULL UNIQUE,
    description text COLLATE pg_catalog."default",
    parent_id integer,
    levels integer
);

CREATE TABLE IF NOT EXISTS public.workers
(
    worker_id serial PRIMARY KEY,
    full_name character varying(255) COLLATE pg_catalog."default" NOT NULL,
    birthdate date,
    phone character varying(20) COLLATE pg_catalog."default" UNIQUE,
    office character varying(10) COLLATE pg_catalog."default" NOT NULL,
    email character varying(255) COLLATE pg_catalog."default" NOT NULL,
    is_subdepartment_head boolean DEFAULT false,
    job_position character varying(255) COLLATE pg_catalog."default" NOT NULL,
    subdepartment_name character varying(255) COLLATE pg_catalog."default" NOT NULL 
	REFERENCES subdepartments(subdepartment_name)
);

CREATE TABLE IF NOT EXISTS public.workers_events
(
    worker_id integer NOT NULL REFERENCES workers(worker_id),
    event_id integer NOT NULL REFERENCES events(event_id),
    CONSTRAINT workers_events_pkey PRIMARY KEY (worker_id, event_id)
);

CREATE TABLE IF NOT EXISTS public.documents
(
    document_id serial PRIMARY KEY,
	title varchar(255) NOT NULL,
    date_approval date NOT NULL,
    date_edit date,
    status character varying(255) COLLATE pg_catalog."default",
    document_type character varying(255) COLLATE pg_catalog."default" NOT NULL,
    field character varying(255) COLLATE pg_catalog."default",
    author_id integer REFERENCES workers(worker_id)
);

CREATE TABLE document_comments (
	comment_id serial PRIMARY KEY,
	document_id int REFERENCES documents(document_id),
	text text NOT NULL,
	date_created timestamp NOT NULL,
	date_updated timestamp,
	author_id int NOT NULL REFERENCES workers(worker_id)
);

CREATE TABLE IF NOT EXISTS public.workingcalendar
(
    id integer NOT NULL,
    exceptiondate date NOT NULL,
    isworkingday boolean NOT NULL,
    CONSTRAINT workingcalendar_pk PRIMARY KEY (id)
);

COMMENT ON TABLE public.workingcalendar
    IS 'Список дней исключений в производственном календаре';

COMMENT ON COLUMN public.workingcalendar.exceptiondate
    IS 'День-исключение';

COMMENT ON COLUMN public.workingcalendar.isworkingday
    IS '0 - будний день, но законодательно принят выходным; 1 - сб или вс, но является рабочим';
	
CREATE TABLE users (
    id serial NOT NULL,
    name text NOT NULL,
    password text NOT NULL,
    CONSTRAINT "PK_users" PRIMARY KEY (id)
);

CREATE INDEX idx_workers_full_name ON workers (full_name);
CREATE INDEX idx_documents_id ON documents (document_id);
CREATE INDEX idx_document_comments_id ON document_comments (comment_id);
CREATE INDEX idx_users_id ON users (name);
END;