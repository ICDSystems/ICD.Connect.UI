# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [6.5.1] - 2020-06-22
### Changed
 - Failing gracefully and logging when a presenter fails to refresh

## [6.5.0] - 2020-02-21
### Added
 - Added OnButtonHeld event to VtProDPad

## [6.4.1] - 2020-01-20
### Changed
 - Fixed deadlock related to AbstractPresenter showing views inside critical sections

## [6.4.0] - 2019-11-20
### Added
 - Added a MultilineSupport bool property to VTPro labels for automatic newline handling

### Changed
 - Improved validation to prevent the unintentional lazy-loading of component presenters

## [6.3.2] - 2021-01-06
### Changed
 - Fixed a memory leak resulting from bad presenter pooling

## [6.3.1] - 2019-10-17
### Changed
 - Fixed a bug that was preventing list moving feedback from working

## [6.3.0] - 2019-10-08
### Added
 - Added VtProCircularGauge
 
### Changed
 - Renamed "Guage" to "Gauge"
 - Small SRL performance improvements

## [6.2.0] - 2019-09-16
### Added
 - Added ButtonListItem struct and methods for updating button list labels and icons in one pass
 
### Changed
 - Potential performance improvements when populating lists

## [6.1.0] - 2019-07-09
### Added
 - Added LazyLoadPresenter overload for simpler casting

### Changed
 - Fixed string formatting bug when getting presenter via binding
 - Significantly improved thread safety when instantiating presenters via bindings
 - Improved thread safety when instantiating views via presenters

## [6.0.0] - 2019-02-21
### Added
 - Added view/presenter bindings for automatically mapping interfaces to concretes
 - Added view/presenter interface binding cache
 - AbstractViewFactory provides a default method for instantiating a view by interface
 - AbstractNavigationController provides a default method for instantiating a presenter by interface

## [5.1.4] - 2020-09-23
### Changed
 - Fixed a bug where SingleVisibility nodes would throw exceptions on visibility changes

## [5.1.3] - 2020-09-17
### Changed
 - Fixed a bug where SingleVisibility nodes did not correctly account for deeply nested children when updating visibility

## [5.1.2] - 2020-09-03
### Changed
 - Fixed a StackOverflow exception related to nested SingleVisibilityNodes

## [5.1.1] - 2019-07-09
### Changed
 - Fixed a bug in ScrollToItem method in AbstractVProList by sending a false command then the real command in order to bypass the cache

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
