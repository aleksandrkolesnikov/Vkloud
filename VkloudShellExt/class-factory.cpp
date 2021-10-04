#include "pch.h"
#include "class-factory.h"
#include "vkloud-menu-ext.h"


extern long g_dllRefCounter;

ClassFactory::ClassFactory()
    : refCounter{ 1 }
{
    InterlockedIncrement(&g_dllRefCounter);
}

ClassFactory::~ClassFactory()
{
    InterlockedDecrement(&g_dllRefCounter);
}

IFACEMETHODIMP ClassFactory::QueryInterface(REFIID riid, void** ppv)
{
    HRESULT result = S_OK;

    if (IsEqualIID(IID_IUnknown, riid) || IsEqualIID(IID_IClassFactory, riid))
    {
        *ppv = static_cast<IUnknown*>(this);
        AddRef();
    }
    else
    {
        result = E_NOINTERFACE;
        *ppv = nullptr;
    }

    return result;
}

IFACEMETHODIMP_(ULONG) ClassFactory::AddRef()
{
    return InterlockedIncrement(&refCounter);
}

IFACEMETHODIMP_(ULONG) ClassFactory::Release()
{
    const auto ref = InterlockedDecrement(&refCounter);
    if (ref == 0)
    {
        delete this;
    }

    return ref;
}

IFACEMETHODIMP ClassFactory::CreateInstance(IUnknown* outer, REFIID riid, void** ppv)
{
    HRESULT result = CLASS_E_NOAGGREGATION;

    if (outer == nullptr)
    {
        auto ext = new VkloudMenuExtention();
        result = ext->QueryInterface(riid, ppv);
        ext->Release();
    }

    return result;
}

IFACEMETHODIMP ClassFactory::LockServer(BOOL lock)
{
    if (lock)
    {
        InterlockedIncrement(&g_dllRefCounter);
    }
    else
    {
        InterlockedDecrement(&g_dllRefCounter);
    }

    return S_OK;
}
