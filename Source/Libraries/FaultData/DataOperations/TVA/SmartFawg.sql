WITH cte AS
(
	SELECT
		Lines.Lines_Id,
		Lines.fromBusNumber,
		Lines.toBusNumber,
		Buses.VoltageValue,
		Lines.LengthMiles,
		Lines.PosSeqResistance,
		Lines.PosSeqReactance,
		Lines.ZeroSeqResistance,
		Lines.ZeroSeqReactance
	FROM
		Lines JOIN
		Buses ON Lines.fromBuses_Id = Buses.Buses_Id JOIN
		Branches ON Lines.Branches_Id = Branches.Branches_Id
	WHERE
		Lines.TransLineNumber = {0} AND
		Branches.Description = {1} AND
		Lines.ISD <= {2} AND
		Lines.OSD >= {2}
	UNION ALL
	SELECT
		Lines.Lines_Id,
		cte.fromBusNumber,
		CASE cte.toBusNumber
			WHEN Lines.fromBusNumber THEN Lines.toBusNumber
			WHEN Lines.toBusNumber THEN Lines.fromBusNumber
		END AS toBusNumber,
		cte.VoltageValue,
		CONVERT(DECIMAL(18, 4), cte.LengthMiles + Lines.LengthMiles) AS LengthMiles,
		CONVERT(DECIMAL(18, 4), cte.PosSeqResistance + Lines.PosSeqResistance) AS PosSeqResistance,
		CONVERT(DECIMAL(18, 4), cte.PosSeqReactance + Lines.PosSeqReactance) AS PosSeqReactance,
		CONVERT(DECIMAL(18, 4), cte.ZeroSeqResistance + Lines.ZeroSeqResistance) AS ZeroSeqResistance,
		CONVERT(DECIMAL(18, 4), cte.ZeroSeqReactance + Lines.ZeroSeqReactance) AS ZeroSeqReactance
	FROM
		cte JOIN
		Lines ON
			cte.Lines_Id <> Lines.Lines_Id AND
			(
				cte.toBusNumber = Lines.fromBusNumber OR
				cte.toBusNumber = Lines.toBusNumber
			) JOIN
		Branches ON Lines.Branches_Id = Branches.Branches_Id
	WHERE
		Lines.TransLineNumber = {0} AND
		Branches.Description = {1} AND
		Lines.ISD <= {2} AND
		Lines.OSD >= {2}
)
SELECT TOP 1
	Lines_Id,
	fromBusNumber,
	toBusNumber,
	VoltageValue,
	LengthMiles,
	(PosSeqResistance / 100) * (VoltageValue * VoltageValue / 100)  AS PosSeqResistance,
	(PosSeqReactance / 100) * (VoltageValue * VoltageValue / 100) AS PosSeqReactance,
	(ZeroSeqResistance / 100) * (VoltageValue * VoltageValue / 100) AS ZeroSeqResistance,
	(ZeroSeqReactance / 100) * (VoltageValue * VoltageValue / 100) AS ZeroSeqReactance
FROM cte
ORDER BY LengthMiles DESC