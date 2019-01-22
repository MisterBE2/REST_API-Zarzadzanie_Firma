-- phpMyAdmin SQL Dump
-- version 4.7.7
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Czas generowania: 22 Sty 2019, 03:40
-- Wersja serwera: 10.1.30-MariaDB
-- Wersja PHP: 7.2.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Baza danych: `restapi-firm`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `messages`
--

CREATE TABLE `messages` (
  `id` int(11) NOT NULL,
  `to_user_id` int(11) NOT NULL,
  `from_user_id` int(11) NOT NULL,
  `message` varchar(256) COLLATE utf8_bin NOT NULL,
  `sended` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Zrzut danych tabeli `messages`
--

INSERT INTO `messages` (`id`, `to_user_id`, `from_user_id`, `message`, `sended`) VALUES
(7, 19, 5, 'Bogusiu, proszÄ™ posprzÄ…taj w Å‚aziÄ™ce, bo coÅ› tam siÄ™ wylaÅ‚o.', '2019-01-22 02:19:21'),
(8, 19, 5, 'Tylko szybciutko :)', '2019-01-22 02:19:40'),
(9, 5, 19, 'Dobrze Panie Pawle', '2019-01-22 02:24:37'),
(10, 5, 19, 'JuÅ¼ pÄ™dzÄ™', '2019-01-22 02:24:40'),
(11, 5, 19, '...', '2019-01-22 02:24:49'),
(12, 5, 19, 'PosprzÄ…tane!', '2019-01-22 02:24:56'),
(13, 19, 5, 'Bardzo dobrze, dostaniesz podwyÅ¼kÄ™!', '2019-01-22 02:25:38');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `status`
--

CREATE TABLE `status` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `status` varchar(128) COLLATE utf8_bin NOT NULL,
  `updated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Zrzut danych tabeli `status`
--

INSERT INTO `status` (`id`, `user_id`, `status`, `updated`) VALUES
(7, 5, 'I am the BOSS here!', '2019-01-22 02:32:16'),
(8, 20, 'In the flow of designing :)', '2019-01-21 13:41:18'),
(10, 18, 'I like trains!', '2019-01-22 00:31:37'),
(11, 19, 'I like cleaning', '2019-01-22 02:37:33');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `firstname` varchar(256) COLLATE utf8_bin NOT NULL,
  `lastname` varchar(256) COLLATE utf8_bin NOT NULL,
  `email` varchar(256) COLLATE utf8_bin NOT NULL,
  `password` varchar(2048) COLLATE utf8_bin NOT NULL,
  `permission` tinyint(4) NOT NULL DEFAULT '1',
  `position` varchar(256) COLLATE utf8_bin NOT NULL,
  `created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Zrzut danych tabeli `users`
--

INSERT INTO `users` (`id`, `firstname`, `lastname`, `email`, `password`, `permission`, `position`, `created`, `updated`) VALUES
(5, 'PaweÅ‚', 'Marecki', 'admin', '$2y$10$1WGXVaLviqHkrEMFEPK0AeN42C36RQNvtkQKM/LKDGZCjl1G7Ubmy', 0, 'CEO', '2019-01-10 22:06:53', '2019-01-22 02:31:59'),
(18, 'Wojtek', 'Waflak', 'wojtek.waflak@gmail.com', '$2y$10$jYNakLCuAW0/FQpKIV37G.jN6Ah/8l98OXTTQ9YnSoG5UTxgHxmUi', 1, '3D graphic', '2019-01-21 00:27:25', '2019-01-21 14:07:58'),
(19, 'BogumiÅ‚a', 'Alpaczak', 'bogumila.alpaczak@gmail.com', '$2y$10$HNBZKh4.lXlXECH8YXaGU.oPAzJCzRGGZHS1Q8aVbcPi/OAWdzFIK', 1, 'Cleaning laydy - head', '2019-01-21 00:28:13', '2019-01-22 02:37:24'),
(20, 'Adrianna', 'Cholewka', 'adrianna.cholewka', '$2y$10$oPcHyLObMMe7tbq1IGY19Oqs1M02pTC.luekbEzaLuHt5KzTO.QBG', 1, 'Designer, Proggramer', '2019-01-21 14:39:53', '2019-01-21 14:08:30'),
(22, 'Olek', 'Woroszyn', 'abcd@gmail.com', '$2y$10$dML1r1wUM8VddBEIO/zRsOcFfogiGdsGz1N...Cx7723zdgpsrEPi', 1, 'Worker', '2019-01-22 01:18:54', '2019-01-22 00:18:54');

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `messages`
--
ALTER TABLE `messages`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `status`
--
ALTER TABLE `status`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT dla tabeli `messages`
--
ALTER TABLE `messages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT dla tabeli `status`
--
ALTER TABLE `status`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT dla tabeli `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
