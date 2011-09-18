<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:trx="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">  
	<xsl:output method="xml" indent="yes" encoding="UTF-8" />

	<!-- entry point -->
	<xsl:template match="/">
		<results>
		  <!-- this is where we'd need to add a category predicate -->
		  <xsl:apply-templates select="//trx:TestMethod[../trx:TestCategory/trx:TestCategoryItem/@TestCategory='Spec Test']" >
			<xsl:sort select="@className" />
		  </xsl:apply-templates>
		</results>
	</xsl:template>

	<!-- report a test method along with its class name and looked up outcome -->
	<xsl:template match="trx:TestMethod">
		<!-- let's strip the class name down to just the class name -->
		<xsl:variable name="className">
			<xsl:call-template name="removePath">
				<!-- remove the gubbins after the class name by splitting on the first comma -->
				<xsl:with-param name="className" select="substring-before(@className, ',')" />
			</xsl:call-template>
		</xsl:variable>
		<result>
			<className><xsl:value-of select="$className" /></className>
			<method><xsl:value-of select="translate(@name, '_', ' ')" /></method>
			<outcome><xsl:value-of select="//trx:UnitTestResult[@testName=current()/@name]/@outcome" /></outcome>
		</result>
	</xsl:template>

	<xsl:template name="removePath">
		<xsl:param name="className" />
		<!-- recurse through until we don't have no stinkin' full stops -->
		<xsl:choose>
			<!-- strip everything up to and including the first full stop -->
			<xsl:when test="contains($className, '.')">
				<xsl:call-template name="removePath">
					<xsl:with-param name="className" select="substring-after($className, '.')" />
				</xsl:call-template>
			</xsl:when>
			<!-- whoo-hoo, we've done it -->
			<xsl:otherwise><xsl:value-of select="$className" /></xsl:otherwise>
		</xsl:choose>
	</xsl:template>
  
</xsl:stylesheet>