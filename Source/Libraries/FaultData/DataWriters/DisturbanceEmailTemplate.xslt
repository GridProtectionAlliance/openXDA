<?xml version="1.0" ?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" />

    <xsl:template match="/">
        <html>
        <head>
            <title>Disturbance detected on a whole lot of lines</title>

            <style>
                th, td {
                  border-spacing: 0;
                  border-collapse: collapse;
                  padding-left: 5px;
                  padding-right: 5px
                }

                .disturbance-details {
                  margin-left: 1cm
                }

                .disturbance-header {
                  font-size: 120%;
                  font-weight: bold;
                  text-decoration: underline
                }

                table {
                  border-spacing: 0;
                  border-collapse: collapse
                }

                table.left tr th, table.left tr td {
                  border: 1px solid black
                }

                table.center tr th, table.center tr td {
                  border: 1px solid black;
                  text-align: center
                }
            </style>
        </head>
        <body>
            <xsl:for-each select="/EventDetail/DisturbanceGroups/DisturbanceGroup">
                <span class="disturbance-header">Disturbance <xsl:value-of select="@num" /></span> - <format type="System.DateTime" spec="yyyy-MM-dd HH:mm:ss.fffffff"><xsl:value-of select="Disturbance[1]/StartTime" /></format>
                <div class="disturbance-details">
                    <table>
                        <xsl:for-each select="Disturbance[not(FileName=preceding-sibling::*/FileName)]">
                            <tr>
                                <td><xsl:if test="position() = 1">DFRs:</xsl:if></td>
                                <td><xsl:value-of select="MeterKey" /> at <xsl:value-of select="StationName" /> triggered at <format type="System.DateTime" spec="HH:mm:ss.fffffff"><xsl:value-of select="EventStartTime" /></format></td>
                            </tr>
                        </xsl:for-each>

                        <tr>
                            <td>&amp;nbsp;</td>
                            <td>&amp;nbsp;</td>
                        </tr>

                        <xsl:for-each select="Disturbance[not(FileName=preceding-sibling::*/FileName)]">
                            <tr>
                                <td><xsl:if test="position() = 1">Files:</xsl:if></td>
                                <td><xsl:value-of select="FileName" /></td>
                            </tr>
                        </xsl:for-each>
                    </table>

                    <br />

                    <table class="left">
                        <tr>
                            <td style="text-align: center">Meter</td>
                            <td style="text-align: center">Line</td>
                            <td style="text-align: center">Type</td>
                            <td style="text-align: center">Phase</td>
                            <td style="text-align: center">Magnitude (p.u.)</td>
                            <td style="text-align: center">Duration (cycles)</td>
                            <td style="text-align: center">Event ID</td>
                        </tr>

                        <xsl:for-each select="Disturbance">
                            <tr>
                                <td><xsl:value-of select="StationName" /> - <xsl:value-of select="MeterKey" /></td>
                                <td><xsl:value-of select="LineName" /></td>
                                <td><xsl:value-of select="DisturbanceType" /></td>
                                <td><xsl:value-of select="Phase" /></td>
                                <td><format type="System.Double" spec="0.00"><xsl:value-of select="PerUnitMagnitude" /></format></td>
                                <td><format type="System.Double" spec="0.00"><xsl:value-of select="DurationCycles" /></format></td>
                                <td>
                                    <a>
                                        <xsl:attribute name="href">http://pqdashboard/openSEE.aspx?eventid=<xsl:value-of select="EventID" />&amp;faultcurves=1</xsl:attribute>
                                        <xsl:value-of select="EventID" />
                                    </a>
                                </td>
                            </tr>
                        </xsl:for-each>
                    </table>
                </div>

                <br />
            </xsl:for-each>
        </body>
    </html>
</xsl:template>

</xsl:stylesheet>