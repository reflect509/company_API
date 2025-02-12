BEGIN;

CREATE TABLE IF NOT EXISTS public.events
(
    event_id serial PRIMARY KEY,
    event_name character varying(255) NOT NULL,
    event_type character varying(255) NOT NULL,
    status character varying(255) NOT NULL,
    date timestamp without time zone,
    description text
);

CREATE TABLE IF NOT EXISTS public.subdepartments
(
    subdepartment_id serial PRIMARY KEY,
    subdepartment_name character varying(255) NOT NULL UNIQUE,
    description text,
    parent_id integer
);

CREATE TABLE IF NOT EXISTS public.workers
(
    worker_id serial PRIMARY KEY,
    full_name character varying(255) NOT NULL,
    birthdate date,
    phone character varying(20) UNIQUE,
    office character varying(10) NOT NULL,
    email character varying(255) NOT NULL,
    is_subdepartment_head boolean DEFAULT false,
    job_position character varying(255) NOT NULL,
    subdepartment_name character varying(255) NOT NULL 
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
    date_edit date NOT NULL,
    status character varying(255),
    document_type character varying(255) NOT NULL,
    field character varying(255),
    author_id integer REFERENCES workers(worker_id)
);

CREATE TABLE document_comments (
	comment_id serial PRIMARY KEY,
	document_id int NOT NULL REFERENCES documents(document_id),
	text text NOT NULL,
	date_created timestamp NOT NULL,
	date_updated timestamp NOT NULL,
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
	
CREATE TABLE app_users (
    user_id serial PRIMARY KEY,
    user_name text NOT NULL,
    user_password text NOT NULL
);

CREATE INDEX idx_workers_full_name ON workers (full_name);
CREATE INDEX idx_documents_id ON documents (document_id);
CREATE INDEX idx_document_comments_id ON document_comments (comment_id);
CREATE INDEX idx_users_id ON app_users (user_name);

END;