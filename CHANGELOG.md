# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.0-preview.1] - 2021-05-04
### Added
- Added UPM support
- Added support for Unity 2019.4

## [1.0.0-preview.2] - 2021-05-05
### Fixed
- Fixed typos in recent README changes.
- Fixed import errors for Samples

## [1.0.0-preview.3] - 2021-05-06
### Fixed
- Lighting settings on Affective Video demo rebuilt for Unity 2019.4.x
- Fixed missing event data and empty recording status

## [1.0.0-preview.4] - 2021-05-10
### Changed
- Data Points demo has new buttons to show how to start and stop recording data.
- Changed the layer for tracked objects on EmteqVREyeManager

## [1.0.0-preview.5] - 2021-05-17
### Fixed
- Unity would hang after calibration because thread processes were not being properly disposed.
- Missing serialisation data on messages sent to SuperVision
- Missing data on Affective Video demo
### Changed
- Calibration now shows ExpressionType and FaceSide as strings on the session json file rather than an enum value.
