create database Warehouse;
use Warehouse;

create table Requests
(
  id int AUTO_INCREMENT primary KEY,
  required_quantity int,
  work_type ENUM('Выгрузка на склад', 'Выгрузка со склада', 'Доставка в цех сборки', 'Доставка в цех тестирования', 'Доставка в цех литья'),
  time_to_complete ENUM('1ч', '2ч', '3ч', '4ч', '5ч'),
  request_status int DEFAULT 0,
  material_id varchar(5) references Materials(id),
  executor_id int references Executors(id)
);

create table Materials
(
  id varchar(5) primary KEY,
  material_name varchar(15),
  stock_quantity int,
  measurement_unit ENUM('шт', 'м²', 'кг')
);

create table Executors
(
  id int AUTO_INCREMENT primary KEY,
  executor_name varchar(25)
);

create table Auths
(
  id int AUTO_INCREMENT primary KEY,
  login varchar(15) not null,
  password varchar(15) not null
);