#include "Stdafx.h"
#include "Shaders.h"

namespace IgneelD3D10 {
	
	CbHandle^ CreateHandle(ID3D10Device * device, int buffSize)
	{
		//Create the Constant Buffer
		D3D10_BUFFER_DESC cbDesc;
		ZeroMemory(&cbDesc, sizeof(D3D10_BUFFER_DESC));
		cbDesc.ByteWidth = buffSize;
		cbDesc.Usage = D3D10_USAGE_DYNAMIC;
		cbDesc.BindFlags = D3D10_BIND_CONSTANT_BUFFER;
		cbDesc.CPUAccessFlags = D3D10_CPU_ACCESS_WRITE;
		cbDesc.MiscFlags = 0;
				
		ID3D10Buffer* constantBuffer;
		SAFECALL( device->CreateBuffer(&cbDesc,NULL, &constantBuffer));

		return gcnew CbHandle(constantBuffer);
	}

	D3D10Shader::D3D10Shader(ID3D10Device * device,ID3D10DeviceChild* Shader, ShaderCode^ bytecode)
	{
		this->shader = Shader;
		ID3D10ShaderReflection* reflec;	
		auto code = bytecode->Code;
		pin_ptr<byte> pterCode = &code[0];
		D3D10ReflectShader(pterCode, code->Length,&reflec);
		ShaderReflection = reflec;
		ASTReflection = bytecode->Reflection;

		D3D10_SHADER_DESC sd;
		reflec->GetDesc(&sd);
		ConstantBuffers = gcnew array<CBufferShaderBinding>(sd.ConstantBuffers);
		int ibuffer = 0;
		for (int i = 0; i < sd.BoundResources; i++)
		{	
			D3D10_SHADER_INPUT_BIND_DESC bindDesc;
			ZeroMemory(&bindDesc, sizeof(D3D10_SHADER_INPUT_BIND_DESC));
			reflec->GetResourceBindingDesc(i, &bindDesc);

			if(bindDesc.Type == D3D10_SIT_CBUFFER)
			{
				auto cbReflec = reflec->GetConstantBufferByName(bindDesc.Name);
				D3D10_SHADER_BUFFER_DESC bufferDesc;
				ZeroMemory(&bufferDesc , sizeof(D3D10_SHADER_BUFFER_DESC));
				cbReflec->GetDesc(&bufferDesc);

				auto cbBinding = CBufferShaderBinding(CreateHandle(device, bufferDesc.Size), bindDesc.BindPoint, ibuffer );				
				ConstantBuffers[ibuffer] = cbBinding;
				ibuffer++;
			}
		}

	}	

	void D3D10Shader::OnDispose(bool disposing)
	{		
		__super::OnDispose(disposing);
		if(this->shader)
		{			
			this->shader->Release();	
			ShaderReflection->Release();			
			this->shader = NULL;
			ShaderReflection = NULL;
		}			
	}

	D3DVertexShader::D3DVertexShader(ID3D10Device * device, ID3D10VertexShader* vs, ShaderCode^ bytecode):
		D3D10Shader(device, vs,  bytecode) 
	{
		_pvs = vs;			
	}		

	void D3DVertexShader::Set(ID3D10Device* device)
	{		
		for (int i = 0;	i < ConstantBuffers->Length; i++)
		{
			auto bind = ConstantBuffers[i];
			bind.handle->Close();
			ID3D10Buffer* cb = bind.handle->cb; 
			device->VSSetConstantBuffers(bind.bindPoint, 1, &cb);
		}
		 device->VSSetShader(_pvs);
	}

	D3DPixelShader::D3DPixelShader(ID3D10Device * device, ID3D10PixelShader* ps,  ShaderCode^ bytecode)
		:D3D10Shader(device, ps,  bytecode) 
	{
		_pps = ps;	
	}
	
	void D3DPixelShader::Set(ID3D10Device* device)
	{
		auto buffers = ConstantBuffers;
		
		for (int i = 0;	i < buffers->Length; i++)
		{
			auto bind = buffers[i];
			bind.handle->Close();
			ID3D10Buffer* cb = bind.handle->cb; 
			device->PSSetConstantBuffers(bind.bindPoint, 1, &cb);
		}
		device->PSSetShader(_pps);
	}

	D3DGeometryShader::D3DGeometryShader(ID3D10Device * device, ID3D10GeometryShader* shader,  ShaderCode^ bytecode)
		:D3D10Shader(device, shader,  bytecode) 
	{
		this->_shader = shader;
	}
		
	void D3DGeometryShader::Set(ID3D10Device* device)
	{
		auto buffers = ConstantBuffers;
	
		for (int i = 0;	i < buffers->Length; i++)
		{
			auto bind = buffers[i];
			bind.handle->Close();
			ID3D10Buffer* cb = bind.handle->cb;
			device->GSSetConstantBuffers(bind.bindPoint, 1, &cb);
		}
		 device->GSSetShader(_shader);
	}
}