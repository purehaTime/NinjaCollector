# NinjaCollector

This is a bot what can collect pictures from popular sites. 
Version 0.1.0

The system can parse posts from reddit and posting it to telegram

[![Build status](https://github.com/purehaTime/NinjaCollector/actions/workflows/build-plan.yml/badge.svg)](https://github.com/purehaTime/NinjaCollector/actions/workflows/build-plan.yml)


#How it work:
	First fill config files with api keys
	After run with docker compose (docker-compose build && docker-compose up -d).
	Add parser settings and run workers on main page
	Add poster settings and rerun workers on main page
	

#Roadmap:
	1) Delete registration and add admin account to db migration from configs
	2) Add footer with status bar
	3) Move worker to microservice
		a) separate settings for workers and parser\posters
		b) remove return setttings from parsers\posters
	4) Add VK parser\poster