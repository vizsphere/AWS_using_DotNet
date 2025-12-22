### Aws Lambda Layer for Shared Code 

1. Prepare director structure to hold dependencies
In my case it was  layer/bin/dotnet8 and create zip file layer.zip

publish the layer to AWS Lambda

```
aws lambda publish-layer-version --layer-name "MySharedDependencies" --zip-file "fileb://layer.zip" --compatible-runtimes dotnet8 --region eu-west-2 

```

2. Add this to lambda function

	<ItemGroup>
		<Reference Include="SharedLayer">
			<HintPath>path\to\SharedLayer.dll</HintPath>
			<Private>false</Private>
		</Reference>
	</ItemGroup>

## Step should return the arn of the layer created use that arn in the next step

3. Add layer to your Lambda function

```
aws lambda update-function-configuration --function-name Speakers --layers arn:aws:lambda:eu-west-2:765403727935:layer:MySharedDependencies:1 --region eu-west-2

```
