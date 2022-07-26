CREATE DATABASE  IF NOT EXISTS `fy111` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `fy111`;
-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: fy111
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `class`
--

DROP TABLE IF EXISTS `class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class` (
  `id` int NOT NULL AUTO_INCREMENT,
  `code` varchar(45) DEFAULT NULL,
  `name` varchar(45) NOT NULL,
  `ip` char(15) DEFAULT NULL,
  `image` varchar(45) DEFAULT NULL,
  `content` text,
  `signup_enabled` tinyint NOT NULL DEFAULT '0',
  `checkin_enabled` tinyint NOT NULL DEFAULT '0',
  `duration` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class`
--

LOCK TABLES `class` WRITE;
/*!40000 ALTER TABLE `class` DISABLE KEYS */;
INSERT INTO `class` VALUES (1,'HA','高空工作車作業','','HA.jpeg',NULL,1,1,120),(2,'SF','施工架作業','','SF.jpeg',NULL,1,1,120),(3,'SW','下水道工程模擬訓練',NULL,'SW.png',NULL,1,1,75),(4,'GC','矽甲烷更換鋼瓶',NULL,'GC.jpeg',NULL,1,1,60);
/*!40000 ALTER TABLE `class` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `class_checkin`
--

DROP TABLE IF EXISTS `class_checkin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class_checkin` (
  `Member_id` varchar(256) NOT NULL,
  `training_id` int NOT NULL,
  `time` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Member_id`,`training_id`),
  KEY `fk_class_checkin_training1_idx` (`training_id`),
  CONSTRAINT `fk_class_checkin_training1` FOREIGN KEY (`training_id`) REFERENCES `training` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class_checkin`
--

LOCK TABLES `class_checkin` WRITE;
/*!40000 ALTER TABLE `class_checkin` DISABLE KEYS */;
INSERT INTO `class_checkin` VALUES ('547e8f7f-2aae-4194-9214-cfda4313740b',1,'2022-07-01 09:01:12'),('547e8f7f-2aae-4194-9214-cfda4313740b',2,'2022-07-01 01:30:34'),('8334bf96-4fa1-40f3-9099-7257c168311f',2,'2022-07-01 01:35:27');
/*!40000 ALTER TABLE `class_checkin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `class_littleunit`
--

DROP TABLE IF EXISTS `class_littleunit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class_littleunit` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Class_unit_id` int NOT NULL,
  `code` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `image` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_class_littleunit_class_unit1` (`Class_unit_id`),
  CONSTRAINT `fk_class_littleunit_class_unit1` FOREIGN KEY (`Class_unit_id`) REFERENCES `class_unit` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class_littleunit`
--

LOCK TABLES `class_littleunit` WRITE;
/*!40000 ALTER TABLE `class_littleunit` DISABLE KEYS */;
INSERT INTO `class_littleunit` VALUES (1,1,'HA_S_00','高空-剪刀-安全裝備穿戴',NULL),(2,1,'HA_S_01','高空-剪刀-作業前檢點',NULL),(3,1,'HA_S_02','高空-剪刀-修剪路樹作業','HA_S_02.jpeg'),(4,2,'HA_C_00','高空-車載-安全裝備穿戴',NULL),(5,2,'HA_C_01','高空-車載-作業前檢點',NULL),(6,2,'HA_C_02','高空-車載-更換燈泡作業',NULL),(7,3,'SF_U_00','施工架作業-安全裝備穿戴',NULL),(8,3,'SF_U_01','施工架作業-磁磚拼貼及牆壁粉刷','SF_U_01.jpeg'),(9,3,'SF_U_02','施工架作業-磚牆拆除作業',NULL),(10,3,'SF_U_03','施工架作業-壁連座阻力與拆除',NULL),(11,4,'SW_U_00','下水道-下人孔前的準備工作與安全裝備穿戴',NULL),(12,4,'SW_U_01','下水道-下水道清淤作業','SW_U_01.jpeg'),(13,5,'GC_U_00','矽甲烷-鋼瓶置換','GC_U_00.png'),(14,5,'GC_U_01','矽甲烷-鋼瓶操作','GC_U_01.png');
/*!40000 ALTER TABLE `class_littleunit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `class_log`
--

DROP TABLE IF EXISTS `class_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class_log` (
  `Member_id` varchar(256) NOT NULL,
  `Class_id` int NOT NULL,
  `start_time` datetime NOT NULL,
  `end_time` datetime DEFAULT NULL,
  PRIMARY KEY (`Member_id`,`start_time`),
  KEY `fk_class_log_Class1_idx` (`Class_id`),
  CONSTRAINT `fk_class_log_Class1` FOREIGN KEY (`Class_id`) REFERENCES `class` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class_log`
--

LOCK TABLES `class_log` WRITE;
/*!40000 ALTER TABLE `class_log` DISABLE KEYS */;
INSERT INTO `class_log` VALUES ('547e8f7f-2aae-4194-9214-cfda4313740b',1,'2022-07-01 09:01:12','2022-07-01 10:01:34'),('547e8f7f-2aae-4194-9214-cfda4313740b',1,'2022-07-01 13:31:12','2022-07-01 14:31:12'),('8334bf96-4fa1-40f3-9099-7257c168311f',1,'2022-07-01 13:31:12','2022-07-01 14:31:12'),('8334bf96-4fa1-40f3-9099-7257c168311f',1,'2022-07-26 14:31:39','2022-07-26 14:32:01'),('8334bf96-4fa1-40f3-9099-7257c168311f',1,'2022-07-26 14:41:58','2022-07-26 14:42:44'),('8334bf96-4fa1-40f3-9099-7257c168311f',2,'2022-07-26 14:42:34','2022-07-26 14:42:52');
/*!40000 ALTER TABLE `class_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `class_signup`
--

DROP TABLE IF EXISTS `class_signup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class_signup` (
  `Member_id` varchar(256) NOT NULL,
  `training_id` int NOT NULL,
  PRIMARY KEY (`Member_id`,`training_id`),
  KEY `fk_class_signup_training1_idx` (`training_id`),
  CONSTRAINT `fk_class_signup_training1` FOREIGN KEY (`training_id`) REFERENCES `training` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class_signup`
--

LOCK TABLES `class_signup` WRITE;
/*!40000 ALTER TABLE `class_signup` DISABLE KEYS */;
INSERT INTO `class_signup` VALUES ('547e8f7f-2aae-4194-9214-cfda4313740b',1),('8334bf96-4fa1-40f3-9099-7257c168311f',1),('ec9b7196-725e-458b-a612-faf238d165cf',1),('547e8f7f-2aae-4194-9214-cfda4313740b',2),('8334bf96-4fa1-40f3-9099-7257c168311f',2),('ec9b7196-725e-458b-a612-faf238d165cf',2);
/*!40000 ALTER TABLE `class_signup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `class_unit`
--

DROP TABLE IF EXISTS `class_unit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class_unit` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Class_id` int NOT NULL,
  `code` varchar(45) DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `image` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_class_unit_class1` (`Class_id`),
  CONSTRAINT `fk_class_unit_class1` FOREIGN KEY (`Class_id`) REFERENCES `class` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class_unit`
--

LOCK TABLES `class_unit` WRITE;
/*!40000 ALTER TABLE `class_unit` DISABLE KEYS */;
INSERT INTO `class_unit` VALUES (1,1,'HA_S','高空工作車作業-剪刀式工作車作業情境單元','HA_S.jpeg'),(2,1,'HA_C','高空工作車作業-車載式工作車作業情境單元',NULL),(3,2,'SF_U','施工架作業單元','SF_U.jpeg'),(4,3,'SW_U','下水道工程單元','SW_U.png'),(5,4,'GC_U','矽甲烷更換鋼瓶單元','GC_U.jpeg');
/*!40000 ALTER TABLE `class_unit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `class_unit_app`
--

DROP TABLE IF EXISTS `class_unit_app`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class_unit_app` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Class_unit_id` int NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  `application` varchar(45) DEFAULT NULL,
  `content` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_table1_Class_unit1_idx` (`Class_unit_id`),
  CONSTRAINT `fk_table1_Class_unit1` FOREIGN KEY (`Class_unit_id`) REFERENCES `class_unit` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class_unit_app`
--

LOCK TABLES `class_unit_app` WRITE;
/*!40000 ALTER TABLE `class_unit_app` DISABLE KEYS */;
INSERT INTO `class_unit_app` VALUES (1,1,'Android測試檔案','HA_S_android.exe','Android測試用'),(2,1,'IOS測試檔案','HA_S_ios.exe','IOS測試用');
/*!40000 ALTER TABLE `class_unit_app` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `class_unit_ckpt`
--

DROP TABLE IF EXISTS `class_unit_ckpt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class_unit_ckpt` (
  `Class_unit_id` int NOT NULL,
  `CKPT_id` varchar(45) NOT NULL,
  `content` text,
  PRIMARY KEY (`Class_unit_id`,`CKPT_id`),
  KEY `fk_Class_unit_ckpt_Class_unit1_idx` (`Class_unit_id`),
  CONSTRAINT `fk_Class_unit_ckpt_Class_unit1` FOREIGN KEY (`Class_unit_id`) REFERENCES `class_unit` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class_unit_ckpt`
--

LOCK TABLES `class_unit_ckpt` WRITE;
/*!40000 ALTER TABLE `class_unit_ckpt` DISABLE KEYS */;
INSERT INTO `class_unit_ckpt` VALUES (1,'CKPT_01','檢查點1'),(1,'CKPT_02','檢查點2'),(1,'CKPT_03','檢查點3'),(1,'CKPT_04','檢查點4'),(1,'CKPT_05','檢查點5'),(1,'CKPT_11','檢查點11'),(1,'CKPT_12','檢查點12'),(1,'CKPT_13','檢查點13'),(1,'CKPT_14','檢查點14'),(1,'CKPT_15','檢查點15');
/*!40000 ALTER TABLE `class_unit_ckpt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `device`
--

DROP TABLE IF EXISTS `device`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `device` (
  `id` int NOT NULL AUTO_INCREMENT,
  `icon` varchar(45) DEFAULT NULL,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `device`
--

LOCK TABLES `device` WRITE;
/*!40000 ALTER TABLE `device` DISABLE KEYS */;
INSERT INTO `device` VALUES (1,NULL,'PC'),(2,NULL,'Android'),(3,NULL,'iOS');
/*!40000 ALTER TABLE `device` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `login_app`
--

DROP TABLE IF EXISTS `login_app`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `login_app` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `application` varchar(45) DEFAULT NULL,
  `content` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `login_app`
--

LOCK TABLES `login_app` WRITE;
/*!40000 ALTER TABLE `login_app` DISABLE KEYS */;
INSERT INTO `login_app` VALUES (1,'Android測試檔','Android_login.exe','Android測試用'),(2,'IOS測試檔','IOS_login.exe','IOS測試用');
/*!40000 ALTER TABLE `login_app` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `login_log`
--

DROP TABLE IF EXISTS `login_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `login_log` (
  `Member_id` varchar(256) NOT NULL,
  `Device_type` int NOT NULL,
  `start_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `end_time` datetime DEFAULT NULL,
  PRIMARY KEY (`Member_id`,`start_time`),
  KEY `fk_Login_Log_Device1_idx` (`Device_type`),
  CONSTRAINT `fk_Login_Log_Device1` FOREIGN KEY (`Device_type`) REFERENCES `device` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `login_log`
--

LOCK TABLES `login_log` WRITE;
/*!40000 ALTER TABLE `login_log` DISABLE KEYS */;
INSERT INTO `login_log` VALUES ('36d6f44a-147c-4c7b-8e10-3450bb71d31c',1,'2022-07-26 14:15:50','2022-07-26 14:15:53'),('547e8f7f-2aae-4194-9214-cfda4313740b',1,'2022-07-01 09:00:00','2022-07-01 15:00:00'),('765059fb-0aa9-427d-8700-e656272663f6',1,'2022-07-26 14:16:10','2022-07-26 14:16:13'),('8334bf96-4fa1-40f3-9099-7257c168311f',1,'2022-07-01 13:00:00','2022-07-01 15:00:00'),('8334bf96-4fa1-40f3-9099-7257c168311f',1,'2022-07-26 14:16:20','2022-07-26 14:16:22'),('8334bf96-4fa1-40f3-9099-7257c168311f',1,'2022-07-26 14:31:34',NULL),('8f95e107-c26c-4406-b30c-6f249bdcafb7',1,'2022-07-26 14:16:01','2022-07-26 14:16:06');
/*!40000 ALTER TABLE `login_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `occdisaster`
--

DROP TABLE IF EXISTS `occdisaster`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `occdisaster` (
  `Code` varchar(45) NOT NULL,
  `content` text,
  PRIMARY KEY (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `occdisaster`
--

LOCK TABLES `occdisaster` WRITE;
/*!40000 ALTER TABLE `occdisaster` DISABLE KEYS */;
INSERT INTO `occdisaster` VALUES ('DD_000','作業中無任何疏失'),('DD_001','職災1'),('DD_002','職災2'),('DD_003','職災3'),('DD_004','職災4'),('DD_005','職災5'),('DD_006','職災6');
/*!40000 ALTER TABLE `occdisaster` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operation_checkpoint`
--

DROP TABLE IF EXISTS `operation_checkpoint`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `operation_checkpoint` (
  `operation_littleunit_log_id` int NOT NULL,
  `CKPT_id` varchar(45) NOT NULL,
  `PointType` int DEFAULT NULL,
  PRIMARY KEY (`operation_littleunit_log_id`,`CKPT_id`),
  KEY `fk_operation_checkpoint_operation_littleunit_log1_idx` (`operation_littleunit_log_id`),
  CONSTRAINT `fk_operation_checkpoint_operation_littleunit_log1` FOREIGN KEY (`operation_littleunit_log_id`) REFERENCES `operation_littleunit_log` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operation_checkpoint`
--

LOCK TABLES `operation_checkpoint` WRITE;
/*!40000 ALTER TABLE `operation_checkpoint` DISABLE KEYS */;
INSERT INTO `operation_checkpoint` VALUES (1,'CKPT_01',1),(1,'CKPT_02',0),(1,'CKPT_03',0),(1,'CKPT_04',1),(1,'CKPT_05',1),(2,'CKPT_11',0),(2,'CKPT_12',1),(2,'CKPT_13',0),(2,'CKPT_14',-1),(2,'CKPT_15',1),(7,'CKPT_01',1),(7,'CKPT_02',0),(7,'CKPT_03',0),(7,'CKPT_04',1),(7,'CKPT_05',1),(8,'CKPT_11',0),(8,'CKPT_12',1),(8,'CKPT_13',0),(8,'CKPT_14',-1),(8,'CKPT_15',1);
/*!40000 ALTER TABLE `operation_checkpoint` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operation_littleunit_log`
--

DROP TABLE IF EXISTS `operation_littleunit_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `operation_littleunit_log` (
  `id` int NOT NULL AUTO_INCREMENT,
  `operation_log_id` int NOT NULL,
  `littleunit_code` varchar(45) DEFAULT NULL,
  `score` int DEFAULT NULL,
  `pass` tinyint DEFAULT NULL,
  `start_time` datetime DEFAULT NULL,
  `end_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_operation_checkpoint_operation_log1` (`operation_log_id`),
  CONSTRAINT `fk_operation_checkpoint_operation_log1` FOREIGN KEY (`operation_log_id`) REFERENCES `operation_unit_log` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operation_littleunit_log`
--

LOCK TABLES `operation_littleunit_log` WRITE;
/*!40000 ALTER TABLE `operation_littleunit_log` DISABLE KEYS */;
INSERT INTO `operation_littleunit_log` VALUES (1,1,'HA_S_00',80,0,'2022-07-01 09:00:00','2022-07-01 10:30:00'),(2,1,'HA_S_01',20,0,'2022-07-01 13:30:00','2022-07-01 15:00:00'),(3,1,'HA_S_02',0,0,NULL,NULL),(4,2,'HA_S_00',0,0,NULL,NULL),(5,2,'HA_S_01',0,0,NULL,NULL),(6,2,'HA_S_02',0,0,NULL,NULL),(7,3,'HA_S_00',80,0,'2022-07-05 14:00:00','2022-07-05 15:30:00'),(8,3,'HA_S_01',20,0,'2022-07-05 16:00:00','2022-07-05 17:30:00'),(9,3,'HA_S_02',0,0,NULL,NULL);
/*!40000 ALTER TABLE `operation_littleunit_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operation_occdisaster`
--

DROP TABLE IF EXISTS `operation_occdisaster`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `operation_occdisaster` (
  `operation_littleunit_log_id` int NOT NULL,
  `OccDisaster_code` varchar(45) NOT NULL,
  PRIMARY KEY (`operation_littleunit_log_id`,`OccDisaster_code`),
  CONSTRAINT `fk_table1_operation_littleunit_log1` FOREIGN KEY (`operation_littleunit_log_id`) REFERENCES `operation_littleunit_log` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operation_occdisaster`
--

LOCK TABLES `operation_occdisaster` WRITE;
/*!40000 ALTER TABLE `operation_occdisaster` DISABLE KEYS */;
INSERT INTO `operation_occdisaster` VALUES (1,'DD_001'),(1,'DD_002'),(2,'DD_003'),(2,'DD_004'),(7,'DD_001'),(7,'DD_002'),(8,'DD_003'),(8,'DD_004');
/*!40000 ALTER TABLE `operation_occdisaster` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operation_unit_log`
--

DROP TABLE IF EXISTS `operation_unit_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `operation_unit_log` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Member_id` varchar(256) DEFAULT NULL,
  `unit_code` varchar(45) DEFAULT NULL,
  `pass` tinyint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operation_unit_log`
--

LOCK TABLES `operation_unit_log` WRITE;
/*!40000 ALTER TABLE `operation_unit_log` DISABLE KEYS */;
INSERT INTO `operation_unit_log` VALUES (1,'547e8f7f-2aae-4194-9214-cfda4313740b','HA_S',0),(2,'8334bf96-4fa1-40f3-9099-7257c168311f','HA_S',0),(3,'8334bf96-4fa1-40f3-9099-7257c168311f','HA_S',0);
/*!40000 ALTER TABLE `operation_unit_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `timer`
--

DROP TABLE IF EXISTS `timer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `timer` (
  `Member_id` varchar(256) NOT NULL,
  `start_time` datetime NOT NULL,
  `end_time` datetime DEFAULT NULL,
  PRIMARY KEY (`Member_id`,`start_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `timer`
--

LOCK TABLES `timer` WRITE;
/*!40000 ALTER TABLE `timer` DISABLE KEYS */;
/*!40000 ALTER TABLE `timer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `training`
--

DROP TABLE IF EXISTS `training`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `training` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Class_id` int NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  `date` date DEFAULT NULL,
  `start_time` time DEFAULT NULL,
  `end_time` time DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_training_Class1_idx` (`Class_id`),
  CONSTRAINT `fk_training_Class1` FOREIGN KEY (`Class_id`) REFERENCES `class` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `training`
--

LOCK TABLES `training` WRITE;
/*!40000 ALTER TABLE `training` DISABLE KEYS */;
INSERT INTO `training` VALUES (1,1,'20220701上午場','2022-07-01','09:00:00','12:00:00'),(2,1,'20220701下午場','2022-07-01','13:30:00','16:30:00');
/*!40000 ALTER TABLE `training` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-07-26 15:26:14
