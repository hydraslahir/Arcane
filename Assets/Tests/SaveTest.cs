using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System;
using Save;
namespace Tests
{
    public class SaveTest
    {
        [Test]
        public void LoadScene_WithInvalidPath_NOthrow(){
            Assert.DoesNotThrow(() => SaveInteraction.GetPath("UnJoliChaton.txtt", false));
        }
        [Test]
        public void LoadScene_WithValidPath_NOthrow() {
            Assert.DoesNotThrow(() => SaveInteraction.GetPath("saveTEST.sav", false));
        }

    }
}