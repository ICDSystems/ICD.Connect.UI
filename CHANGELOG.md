# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [6.0.0] - 2019-02-21
### Added
 - Added view/presenter bindings for automatically mapping interfaces to concretes
 - Added view/presenter interface binding cache
 - AbstractViewFactory provides a default method for instantiating a view by interface
 - AbstractNavigationController provides a default method for instantiating a presenter by interface

## [5.1.0] - 2019-04-05
### Added
 - Added method to replace newlines with HTML line breaks
 
### Changed
 - Small optimization when calculating VisibilityNode visibility

## [5.0.0] - 2018-11-20
### Added
 - Added pre-visibility change events for better UX

### Changed
 - Better view control initialization
 - Caching presenter loggers for micro-optimization
 - AbstractListItemFactory takes subscribe/unsubscribe actions via constructor
 - No longer throwing exceptions when trying to show/enable controls with a join of 0

### Removed
 - Removed component presenter and view

## [4.2.0] - 2018-09-14
### Changed
 - Various performance improvements

## [4.1.0] - 2018-04-27
### Added
 - Added AsyncRefreshQueue for limiting the number of subpage refreshes

## [4.0.0] - 2018-04-27
### Added
 - Added configurable OnHeld event to button controls
 
### Changed
 - Removed suffix from assembly name
