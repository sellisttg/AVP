CREATE DATABASE  IF NOT EXISTS `avp2017` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `avp2017`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 13.64.66.218    Database: avp2017
-- ------------------------------------------------------
-- Server version	5.6.34

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
-- Table structure for table `incident`
--

DROP TABLE IF EXISTS `incident`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `incident` (
  `incidentid` int(11) NOT NULL AUTO_INCREMENT,
  `incidentname` varchar(500) DEFAULT NULL,
  `latitude` float DEFAULT NULL,
  `longitude` float DEFAULT NULL,
  `incidenttype` varchar(200) DEFAULT NULL,
  `incidentradius` int(11) DEFAULT NULL,
  `id` varchar(200) DEFAULT NULL,
  UNIQUE KEY `incidentid` (`incidentid`)
) ENGINE=InnoDB AUTO_INCREMENT=239 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `incidentsubscribers`
--

DROP TABLE IF EXISTS `incidentsubscribers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `incidentsubscribers` (
  `IncidentSubscribersID` int(11) NOT NULL AUTO_INCREMENT,
  `IncidentID` int(11) NOT NULL,
  `UserAddressID` int(11) NOT NULL,
  UNIQUE KEY `IncidentSubscribersID` (`IncidentSubscribersID`),
  KEY `fkuseraddressincidentsubscribers` (`UserAddressID`),
  KEY `fkincidentincidentsubscribers` (`IncidentID`),
  CONSTRAINT `fkincidentincidentsubscribers` FOREIGN KEY (`IncidentID`) REFERENCES `incident` (`incidentid`),
  CONSTRAINT `fkuseraddressincidentsubscribers` FOREIGN KEY (`UserAddressID`) REFERENCES `useraddress` (`UserAddressID`)
) ENGINE=InnoDB AUTO_INCREMENT=555 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notification`
--

DROP TABLE IF EXISTS `notification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `notification` (
  `NotificationID` int(11) NOT NULL AUTO_INCREMENT,
  `Message` varchar(10000) DEFAULT NULL,
  `MessageDateTime` datetime DEFAULT NULL,
  `SendingUserID` int(11) DEFAULT NULL,
  `incidentid` int(11) DEFAULT NULL,
  UNIQUE KEY `NotificationID` (`NotificationID`),
  KEY `fkNotificationUserProfile` (`SendingUserID`),
  KEY `fknotificationincident` (`incidentid`),
  CONSTRAINT `fknotificationincident` FOREIGN KEY (`incidentid`) REFERENCES `incident` (`incidentid`),
  CONSTRAINT `notification_ibfk_1` FOREIGN KEY (`SendingUserID`) REFERENCES `userprofile` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notificationemaillocation`
--

DROP TABLE IF EXISTS `notificationemaillocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `notificationemaillocation` (
  `NotificationID` int(11) DEFAULT NULL,
  `UserEmailLocationID` int(11) DEFAULT NULL,
  UNIQUE KEY `unique_index` (`NotificationID`,`UserEmailLocationID`),
  KEY `fkNotificationEmailLocationUserEmailLocation` (`UserEmailLocationID`),
  CONSTRAINT `notificationemaillocation_ibfk_1` FOREIGN KEY (`NotificationID`) REFERENCES `notification` (`NotificationID`),
  CONSTRAINT `notificationemaillocation_ibfk_2` FOREIGN KEY (`UserEmailLocationID`) REFERENCES `useremaillocation` (`UserEmailLocationID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notificationpushlocation`
--

DROP TABLE IF EXISTS `notificationpushlocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `notificationpushlocation` (
  `NotificationID` int(11) DEFAULT NULL,
  `UserPushLocationID` int(11) DEFAULT NULL,
  UNIQUE KEY `unique_index` (`NotificationID`,`UserPushLocationID`),
  KEY `fkNotificationPushLocationUserPushLocation` (`UserPushLocationID`),
  CONSTRAINT `notificationpushlocation_ibfk_1` FOREIGN KEY (`NotificationID`) REFERENCES `notification` (`NotificationID`),
  CONSTRAINT `notificationpushlocation_ibfk_2` FOREIGN KEY (`UserPushLocationID`) REFERENCES `userpushlocation` (`UserPushLocationID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `notificationsmslocation`
--

DROP TABLE IF EXISTS `notificationsmslocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `notificationsmslocation` (
  `NotificationID` int(11) DEFAULT NULL,
  `UserSMSLocationID` int(11) DEFAULT NULL,
  UNIQUE KEY `unique_index` (`NotificationID`,`UserSMSLocationID`),
  KEY `fkNotificationSMSLocationUserSMSLocation` (`UserSMSLocationID`),
  CONSTRAINT `notificationsmslocation_ibfk_1` FOREIGN KEY (`NotificationID`) REFERENCES `notification` (`NotificationID`),
  CONSTRAINT `notificationsmslocation_ibfk_2` FOREIGN KEY (`UserSMSLocationID`) REFERENCES `usersmslocation` (`UserSMSLocationID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `role` (
  `RoleID` int(11) NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(100) NOT NULL,
  PRIMARY KEY (`RoleName`),
  UNIQUE KEY `RoleID` (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `useraddress`
--

DROP TABLE IF EXISTS `useraddress`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `useraddress` (
  `UserAddressID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) DEFAULT NULL,
  `StreetAddress` varchar(100) DEFAULT NULL,
  `City` varchar(100) DEFAULT NULL,
  `State` varchar(2) DEFAULT NULL,
  `Zip` decimal(9,0) DEFAULT NULL,
  `Latitude` mediumtext,
  `Longitude` mediumtext,
  UNIQUE KEY `UserAddressID` (`UserAddressID`),
  KEY `fkuseraddressuser` (`UserID`),
  CONSTRAINT `fkuseraddressuser` FOREIGN KEY (`UserID`) REFERENCES `userprofile` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `useremaillocation`
--

DROP TABLE IF EXISTS `useremaillocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `useremaillocation` (
  `UserEmailLocationID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) DEFAULT NULL,
  `EmailAddress` varchar(100) DEFAULT NULL,
  `UserAddressID` int(11) DEFAULT NULL,
  UNIQUE KEY `UserEmailLocationID` (`UserEmailLocationID`),
  KEY `fkUserEmailLocationUserProfile` (`UserID`),
  KEY `fkuseremaillocationuseraddress` (`UserAddressID`),
  CONSTRAINT `fkuseremaillocationuseraddress` FOREIGN KEY (`UserAddressID`) REFERENCES `useraddress` (`UserAddressID`),
  CONSTRAINT `useremaillocation_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `userprofile` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `userprofile`
--

DROP TABLE IF EXISTS `userprofile`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userprofile` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(100) NOT NULL,
  `EmailOptIn` tinyint(1) DEFAULT NULL,
  `SMSOptIn` tinyint(1) DEFAULT NULL,
  `PushOptIn` tinyint(1) DEFAULT NULL,
  `PasswordHash` varchar(200) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Username`),
  UNIQUE KEY `UserID` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=119 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `userpushlocation`
--

DROP TABLE IF EXISTS `userpushlocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userpushlocation` (
  `UserPushLocationID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) DEFAULT NULL,
  `PhoneNumber` bigint(20) DEFAULT NULL,
  `UserAddressID` int(11) DEFAULT NULL,
  UNIQUE KEY `UserPushLocationID` (`UserPushLocationID`),
  KEY `fkUserPushLocationUserProfile` (`UserID`),
  KEY `fkuserpushlocationuseraddress` (`UserAddressID`),
  CONSTRAINT `fkuserpushlocationuseraddress` FOREIGN KEY (`UserAddressID`) REFERENCES `useraddress` (`UserAddressID`),
  CONSTRAINT `userpushlocation_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `userprofile` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `userrole`
--

DROP TABLE IF EXISTS `userrole`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userrole` (
  `UserID` int(11) DEFAULT NULL,
  `RoleID` int(11) DEFAULT NULL,
  KEY `fkUserRoleRole` (`RoleID`),
  KEY `fkUserRoleUserProfile` (`UserID`),
  CONSTRAINT `userrole_ibfk_1` FOREIGN KEY (`RoleID`) REFERENCES `role` (`RoleID`),
  CONSTRAINT `userrole_ibfk_2` FOREIGN KEY (`UserID`) REFERENCES `userprofile` (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `usersmslocation`
--

DROP TABLE IF EXISTS `usersmslocation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `usersmslocation` (
  `UserSMSLocationID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) DEFAULT NULL,
  `PhoneNumber` bigint(20) DEFAULT NULL,
  `UserAddressID` int(11) DEFAULT NULL,
  UNIQUE KEY `UserSMSLocationID` (`UserSMSLocationID`),
  KEY `fkUserSMSLocationUserProfile` (`UserID`),
  KEY `fkusersmslocationuseraddress` (`UserAddressID`),
  CONSTRAINT `fkusersmslocationuseraddress` FOREIGN KEY (`UserAddressID`) REFERENCES `useraddress` (`UserAddressID`),
  CONSTRAINT `usersmslocation_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `userprofile` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-03-03 10:55:35
