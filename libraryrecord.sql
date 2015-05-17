CREATE DATABASE  IF NOT EXISTS `library` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `library`;
-- MySQL dump 10.13  Distrib 5.5.16, for Win32 (x86)
--
-- Host: 127.0.0.1    Database: library
-- ------------------------------------------------------
-- Server version	5.5.18

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `borrowed`
--

DROP TABLE IF EXISTS `borrowed`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `borrowed` (
  `Date_Borrowed` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Date_Due` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `Date_Returned` timestamp NULL DEFAULT NULL,
  `Book_id` int(11) NOT NULL,
  `Student_id` int(11) NOT NULL,
  KEY `fk_borrowed_book` (`Book_id`),
  KEY `fk_borrowed_student1` (`Student_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `borrowed`
--

LOCK TABLES `borrowed` WRITE;
/*!40000 ALTER TABLE `borrowed` DISABLE KEYS */;
INSERT INTO `borrowed` VALUES ('2013-04-10 02:03:13','2013-04-17 02:03:13',NULL,16,11),('2013-04-10 06:31:07','2013-04-10 02:39:01','2013-04-10 06:31:07',16,12),('2013-04-10 06:31:07','2013-04-10 06:25:31','2013-04-10 06:31:07',16,12),('2013-04-10 06:28:26','2013-04-17 06:27:52','2013-04-10 06:28:26',16,1),('2013-04-10 07:05:29','2013-04-17 07:05:29',NULL,1,11);
/*!40000 ALTER TABLE `borrowed` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `book`
--

DROP TABLE IF EXISTS `book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `book` (
  `Book_id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(80) NOT NULL,
  `Author` varchar(45) NOT NULL,
  `Edition` varchar(45) NOT NULL,
  `Description` varchar(45) NOT NULL,
  `Type` varchar(45) NOT NULL,
  `Kind` varchar(45) NOT NULL,
  `Num_Of_Copy` int(11) NOT NULL,
  PRIMARY KEY (`Book_id`)
) ENGINE=MyISAM AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `book`
--

LOCK TABLES `book` WRITE;
/*!40000 ALTER TABLE `book` DISABLE KEYS */;
INSERT INTO `book` VALUES (16,'algorithms for visual design using the processing language','Kostas Terzidis','1','Programming algortithms for image processing','Computer','NonFiction',5),(1,'A Tale of Two Cities',' 	Charles Dickens','1','novel','Novel','Fiction',3),(2,'Pre-Algebra ','Margaret Thomas','1','Math Book for High School','Math','NonFiction',2),(3,'Using the Standards: Algebra, Grade 1 ','Terry Huston','1','Math Book for Grade 1','Math','NonFiction',1),(4,'Algebra and Pre-Algebra ','Rebecca Wingard-Nelson','2','Math Book for High School','Math','NonFiction',2),(5,'Beginning Algebra ','Peter D. Frisk','1','Math Book for Grade 1','Math','NonFiction',3),(6,'The Little Prince',' 	Antoine de Saint-Exupéry','1','Novel','Novel','Fiction',2),(7,'And Then There Were None','Agatha Christie','1','Novel','Novel','Fiction',1),(8,'Purely Functional Data Structures','Chris Okasaki','1','Data Structure Book for it','Computer','NonFiction',4),(9,'Data Structures & Algorithms in Java','Robert Lapore','2','Data Structure Book for it','Computer','NonFiction',2),(10,'Programming Microsoft® .NET','Jeff Prosise','2','c# Programming language','Computer','NonFiction',2);
/*!40000 ALTER TABLE `book` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `student` (
  `Student_id` int(11) NOT NULL AUTO_INCREMENT,
  `First_Name` varchar(45) NOT NULL,
  `Middle_Name` varchar(45) NOT NULL,
  `Last_Name` varchar(45) NOT NULL,
  `Course` varchar(45) NOT NULL,
  `Year` int(11) NOT NULL,
  PRIMARY KEY (`Student_id`)
) ENGINE=MyISAM AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
INSERT INTO `student` VALUES (11,'Vincent','Lui','Lim','BS Information Technology',4),(12,'Christie','Lors','Liare','BS Computer Science',1),(1,'Carol','Ng','Balanag','BS Computer Science',3),(2,'Benjolynne','','Sia','BS Information Technology',4),(3,'Kathleen','','Teng','BS Information Technology',4),(4,'Huang','Yi','Wen','BS Information Technology',4),(5,'Hung','Chao','Chiu','BS Information Technology',4),(6,'Christopher','','See','BS Information Technology',4),(7,'Jonathan','','Pua','BS Information Technology',4),(8,'Dominic','Lim','Yao','BS Computer Science',4),(9,'Major','Weising','Tsai','BS Computer Sciecne',4),(10,'Waldemar','','Dy','BS Information Technology',4),(13,'Delwin','','Dy','BS Information Technology',4),(14,'Marvin','Lester','Dee','BS Information Technology',4),(15,'Jerome','','Ng','BS Information Technology',4),(16,'Kimberly','','See','BS Information Technology',4),(17,'Althea','','Chong','BS Management',4),(18,'Theresa','','Cheng','BS Management',4),(19,'Jenny','','Leen','BS Management',4),(20,'Willie','','Felix','BS Information Technology',3);
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-04-10 16:46:34
