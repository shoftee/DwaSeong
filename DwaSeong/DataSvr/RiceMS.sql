CREATE DATABASE  IF NOT EXISTS `ricems` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ricems`;
-- MySQL dump 10.13  Distrib 5.5.16, for Win32 (x86)
--
-- Host: localhost    Database: ricems
-- ------------------------------------------------------
-- Server version	5.5.22

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
-- Table structure for table `character_primarystats`
--

DROP TABLE IF EXISTS `character_primarystats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_primarystats` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `CharacterId` int(11) NOT NULL,
  `Level` int(11) NOT NULL,
  `Job` int(11) NOT NULL,
  `Str` int(11) NOT NULL,
  `Dex` int(11) NOT NULL,
  `Int` int(11) NOT NULL,
  `Luk` int(11) NOT NULL,
  `Hp` int(11) NOT NULL,
  `MaxHp` int(11) NOT NULL,
  `Mp` int(11) NOT NULL,
  `MaxMp` int(11) NOT NULL,
  `Ap` int(11) NOT NULL,
  `Sp` bigint(20) NOT NULL,
  `Exp` int(11) NOT NULL,
  `Fame` int(11) NOT NULL,
  `DemonSlayerAccessory` int(11) NOT NULL,
  `Fatigue` int(11) NOT NULL,
  `BattlePoints` int(11) NOT NULL,
  `BattleExp` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account` (
  `AccountID` int(11) NOT NULL AUTO_INCREMENT,
  `AccountName` varchar(20) NOT NULL,
  `PasswordHash` varchar(32) NOT NULL,
  `Pin` varchar(4) NOT NULL,
  `IsBanned` int(11) NOT NULL,
  `CurrentIP` varchar(15) NOT NULL,
  `CurrentTime` int(11) NOT NULL,
  `Connected` int(11) NOT NULL,
  `IsEmailPending` int(11) NOT NULL,
  `IsTradeBanned` int(11) NOT NULL,
  `BanExpiration` bigint(20) NOT NULL,
  `TradeBanExpiration` bigint(20) NOT NULL,
  `Pic` varchar(20) NOT NULL,
  `Admin` int(11) NOT NULL,
  `BanDescription` text,
  `MacAddress` varchar(12) NOT NULL,
  `HWID` varchar(8) NOT NULL,
  `RecentWorld` int(11) NOT NULL,
  `RecentChannel` int(11) NOT NULL,
  `Conauth` bigint(20) NOT NULL,
  `CreateDate` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`AccountID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_data`
--

DROP TABLE IF EXISTS `character_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_data` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `AccountId` int(11) NOT NULL,
  `Name` varchar(13) NOT NULL,
  `Gender` int(11) NOT NULL,
  `Skin` int(11) NOT NULL,
  `Hair` int(11) NOT NULL,
  `Face` int(11) NOT NULL,
  `Map` int(11) NOT NULL,
  `MapPosition` int(11) NOT NULL,
  `Meso` int(11) NOT NULL,
  `Deleted` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `equips`
--

DROP TABLE IF EXISTS `equips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `equips` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `CharacterId` int(11) NOT NULL,
  `ItemId` int(11) NOT NULL,
  `Position` int(11) NOT NULL,
  `Expiration` bigint(20) NOT NULL,
  `PossibleUpgrades` int(11) NOT NULL,
  `Level` int(11) NOT NULL,
  `Str` int(11) NOT NULL,
  `Dex` int(11) NOT NULL,
  `Int` int(11) NOT NULL,
  `Luk` int(11) NOT NULL,
  `IncHP` int(11) NOT NULL,
  `IncMP` int(11) NOT NULL,
  `Watk` int(11) NOT NULL,
  `Wdef` int(11) NOT NULL,
  `Matk` int(11) NOT NULL,
  `Mdef` int(11) NOT NULL,
  `Accuracy` int(11) NOT NULL,
  `Avoid` int(11) NOT NULL,
  `Speed` int(11) NOT NULL,
  `Jump` int(11) NOT NULL,
  `Owner` varchar(13) DEFAULT NULL,
  `Flag` int(11) NOT NULL,
  `Durability` int(11) NOT NULL,
  `State` int(11) NOT NULL,
  `Enhancements` int(11) NOT NULL,
  `Potential1` int(11) NOT NULL,
  `Potential2` int(11) NOT NULL,
  `Potential3` int(11) NOT NULL,
  `Potential4` int(11) NOT NULL,
  `Potential5` int(11) NOT NULL,
  `SocketMask` int(11) NOT NULL,
  `Socket1` int(11) NOT NULL,
  `Socket2` int(11) NOT NULL,
  `Socket3` int(11) NOT NULL,
  `Origin` text,
  `CreationTime` bigint(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `items`
--

DROP TABLE IF EXISTS `items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `items` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `CharacterId` int(11) NOT NULL,
  `ItemId` int(11) NOT NULL,
  `Position` int(11) NOT NULL,
  `Expiration` bigint(20) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `Owner` varchar(13) DEFAULT NULL,
  `Flag` int(11) NOT NULL,
  `Origin` text,
  `CreationTime` bigint(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_traits`
--

DROP TABLE IF EXISTS `character_traits`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_traits` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `CharacterId` int(11) NOT NULL,
  `Ambition` int(11) NOT NULL,
  `Insight` int(11) NOT NULL,
  `Willpower` int(11) NOT NULL,
  `Diligence` int(11) NOT NULL,
  `Empathy` int(11) NOT NULL,
  `Charm` int(11) NOT NULL,
  `AmbitionGained` int(11) NOT NULL,
  `InsightGained` int(11) NOT NULL,
  `WillpowerGained` int(11) NOT NULL,
  `DiligenceGained` int(11) NOT NULL,
  `EmpathyGained` int(11) NOT NULL,
  `CharmGained` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-07-26 23:11:43
