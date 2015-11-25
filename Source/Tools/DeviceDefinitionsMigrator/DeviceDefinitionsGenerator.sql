SELECT
(
	SELECT
		AssemblyName AS [@assembly],
		TypeName + '.' + MethodName AS [@method]
	FROM FaultLocationAlgorithm
	FOR XML PATH('faultLocation'), TYPE
) AS [analytics],
(
	SELECT
		Meter.AssetKey AS [@id],
		Meter.Name AS [attributes/name],
		Meter.Make AS [attributes/make],
		Meter.Model AS [attributes/model],
		MeterLocation.AssetKey AS [attributes/stationID],
		MeterLocation.Name AS [attributes/stationName],
		CAST(MeterLocation.Latitude AS VARCHAR(10)) AS [attributes/stationLatitude],
		CAST(MeterLocation.Longitude AS VARCHAR(10)) AS [attributes/stationLongitude],
		(
			SELECT
				Line.AssetKey AS [@id],
				MeterLine.LineName AS name,
				CAST(Line.VoltageKV AS VARCHAR(10)) AS voltage,
				CAST(Line.ThermalRating AS VARCHAR(10)) AS rating50F,
				CAST(Line.Length AS VARCHAR(10)) AS length,
				EndStation.AssetKey AS endStationID,
				EndStation.Name AS endStationName,
				CAST(LineImpedance.R1 AS VARCHAR(10)) AS [impedances/R1],
				CAST(LineImpedance.X1 AS VARCHAR(10)) AS [impedances/X1],
				CAST(LineImpedance.R0 AS VARCHAR(10)) AS [impedances/R0],
				CAST(LineImpedance.X0 AS VARCHAR(10)) AS [impedances/X0],
				(
					SELECT CAST('<' + OutputChannel.ChannelKey + '>' + Series.SourceIndexes + '</' + Outputchannel.ChannelKey + '>' AS XML)
					FROM
						Channel JOIN
						Series ON Series.ChannelID = Channel.ID JOIN
						OutputChannel ON OutputChannel.SeriesID = Series.ID
					WHERE
						Channel.MeterID = Meter.ID AND
						Channel.LineID = Line.ID
					ORDER BY LoadOrder
					FOR XML PATH(''), TYPE
				) AS channels
			FROM
				MeterLocation Station JOIN
				MeterLocationLine StationLine ON StationLine.MeterLocationID = Station.ID JOIN
				Line ON StationLine.LineID = Line.ID JOIN
				MeterLocationLine EndStationLine ON EndStationLine.LineID = Line.ID JOIN
				MeterLocation EndStation ON EndStationLine.MeterLocationID = EndStation.ID JOIN
				MeterLine ON MeterLine.LineID = Line.ID JOIN
				LineImpedance ON LineImpedance.LineID = Line.ID
			WHERE
				Station.ID = MeterLocation.ID AND
				EndStation.ID <> MeterLocation.ID AND
				MeterLine.MeterID = Meter.ID
			FOR XML PATH('line'), TYPE
		) AS lines
	FROM
		Meter JOIN
		MeterLocation ON Meter.MeterLocationID = MeterLocation.ID
	FOR XML PATH('device'), TYPE
)
FOR XML PATH('openFLE')